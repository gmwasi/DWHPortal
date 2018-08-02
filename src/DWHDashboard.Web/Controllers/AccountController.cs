using System;
using System.Linq;
using System.Threading.Tasks;
using DWHDashboard.ProfileManagement.Core.Interfaces;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.Web.Helpers;
using DWHDashboard.Web.Services;
using DWHDashboard.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DWHDashboard.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IImpersonatorRepository _impersonatorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<User> userManager, IUserRepository userRepository, IOrganizationRepository organizationRepository, IEmailSender emailSender, IImpersonatorRepository impersonatorRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            _emailSender = emailSender;
            _impersonatorRepository = impersonatorRepository;
        }

        // POST api/account/register
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var impersonator = _impersonatorRepository.GetAll().FirstOrDefault();
            if (impersonator != null) model.ImpersonatorId = impersonator.Id;
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                OrganizationId = model.Organization,
                UserType = model.UserType,
                ImpersonatorId = model.ImpersonatorId,
                Title = model.Title,
                Designation = model.Designation,
                ReasonForAccessing = model.ReasonForAccessing
            };
            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Log.Error(e.ToString());
                return StatusCode(500, e.Message);
            }

            try
            {
                // Send an email with this link
                string callbackUrl = await SendEmailConfirmationTokenAsync(user, "Confirm your account");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Log.Error(e.ToString());
                return Ok("Account created but the confirmation email was not successfully sent. Kindly try to login to resend the email.");
            }

            try
            {
                //send steward email to confirm user
                string stewardCallbackUrl = await SendStewardEmailConfirmationRequestAsync(user, "Confirm " + user.FullName + "'s account");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Log.Error(e.ToString());
                return Ok("Account created but the Steward was not sent a verification email. Kindly contact the administrator.");
            }

            return new OkObjectResult($"Account created. Check your email and confirm your account. You must confirm your email before you can log in.");

        }

        // GET: api/Account/ConfirmEmail
        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest();
            }

            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (!result.Succeeded)
                {
                    return StatusCode(500);
                }
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Log.Error(e.ToString());
                return StatusCode(500, e.Message);
            }
            
        }

        // GET: /Account/ConfirmUser
        [HttpGet]
        public async Task<ActionResult> ConfirmUser(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest();
            }

            //todo complete confirm user functionality
            //var result = await _userManager.ConfirmUserAsync(userId, code);
            bool succeeded = true;
            if (!succeeded)
            {
                return StatusCode(500);
            }
            return Ok();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return Ok();
                }


                // Send reset password email
                var callBackUrl = SendEmailResetPasswordAsync(user, "Reset your password");
                return Ok();
            }

            // If we got this far, something failed, redisplay form
            return BadRequest();
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(500, "Model state invalid");
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Ok();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            AddErrors(result);
            return StatusCode(500, ModelState);
        }


        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.ToString());
            }
        }

        public async Task<string> SendEmailConfirmationTokenAsync(User user, string subject)
        {
            //Generate email token
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var organization = _organizationRepository.FindByKey(user.OrganizationId);

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var callbackUrl = baseUrl + Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = token });
            var loginUrl = baseUrl;

            //todo include datawarehouse image in email sent
            var emailImageUrl = File("Images\\DWHEmailImg.png", "image/jpeg"); ;//Url.Action("DWHEmailImg.png", "Images", "", protocol: Request.Url.Scheme);

            string message = "<p>Dear&nbsp;<strong>" + user.FullName + "</strong>,</p>\r\n" +
                             "<p>Welcome to the NASCOP National EMR Data Warehouse.</p>\r\n" +
                             "<p>Thank you for creating the account below:</p>\r\n" +
                             "<p><em>Title: " + user.Designation + " </em></p>\r\n" +
                             "<p><em>Name: " + user.FullName + " </em></p>\r\n" +
                             "<p><em>Email: " + user.Email + " </em></p>\r\n" +
                             "<p><em>Username: " + user.UserName + " </em></p>\r\n" +
                             "<p><em>Created on: " + DateTime.Now + " </em></p>\r\n" +
                             "<p>In order to gain access to the resource you are required to verify your email by clicking <a href=\"" + callbackUrl + "\">here</a>" +
                             "<p>A separate request has been sent to <strong>" + organization.Name + "\'s</strong> data steward to verify your account and give you access privileges as defined by <strong>" + organization.Name + "</strong></p>\r\n" +
                             "<p>Click  <a href=\"" + loginUrl + "\">here</a> to log in with limited access.</p>\r\n" +
                             "<p>If you have any questions/concerns please contact <strong>" + organization.Name + "\'s</strong> data steward.</p>\r\n" +
                             "<p>Regards,&nbsp;</p>\r\n" +
                             "<p>National EMR Data Warehouse Access Team</p>" +
                             "<p><img src=\"..\\Images\\DWHEmailImg.png\" alt=\"Integrated data warehouse\"></p>";

            await _emailSender.SendEmailAsync(user.Email, subject, message);
            return callbackUrl;
        }

        public async Task<string> SendStewardEmailConfirmationRequestAsync(User user, string subject)
        {
            //Get stewards
            var userConfirmer = _userRepository.GetAll().Where(n => n.UserType == UserType.Steward && n.OrganizationId == user.OrganizationId);

            //Get admin if no steward is configured
            var stewards = userConfirmer.ToList();
            if (stewards.Any() == false)
                stewards = _userRepository.GetAll().Where(n => n.UserType == UserType.Admin).ToList();
            var organization = _organizationRepository.FindByKey(user.OrganizationId);
            //todo configure call back url for steward email
            var callbackUrl = "";

            foreach (var steward in stewards)
            {
                string message = "<p>Dear&nbsp;<strong>" + steward.FullName + "</strong>,</p>\r\n" +
                                 "<p><em>Name: " + user.Title + ", " + user.FullName + " </em></p>\r\n" +
                                 "<p><em>Position: " + user.Designation + " </em></p>\r\n" +
                                 "<p><em>Email: " + user.Email + " </em></p>\r\n" +
                                 "<p><em>Username: " + user.UserName + " </em></p>\r\n" +
                                 "<p><em>Request date and time: " + DateTime.Now + " </em></p>\r\n" +
                                 "<p><em>Reason for access: " + user.ReasonForAccessing + " </em></p>\r\n" +
                                 "<p>The above mentioned has created an account on the Integrated Data Warehouse portal and is requesting affiliation to <strong>" + organization.Name + "\'s</strong> data and access previleges.</p>\r\n" +
                                 "<p>To confirm this affiliation and grant access to this request please log into the National Data Warehouse portal <a href=\"" + callbackUrl + "\">here</a> and grant access.</p>\r\n" +
                                 "<p>If the above individual is not affiliated to your organization you can choose to ignore this message and do nothing.</p>\r\n" +
                                 "<p>If you have any questions/concerns please contact Administrator on Mwenda.Gitonga@thepalladiumgroup.com</p>\r\n<p>&nbsp;</p>\r\n" +
                                 "<p>Regards,&nbsp;</p>\r\n" +
                                 "<p>National EMR Data Warehouse Access Team</p>" +
                                 "<p><img src=\"..\\Images\\DWHEmailImg.png\" alt=\"Integrated data warehouse\"></p>";
                await _emailSender.SendEmailAsync(steward.Email, subject, message);
            }
            return callbackUrl;
        }

        public async Task<string> SendEmailResetPasswordAsync(User user, string subject)
        {
            //Generate reset password token
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var callbackUrl = baseUrl + Url.Action("ResetPassword", "Account", new { userId = user.Id, code = token });

            string message = "<p>Dear&nbsp;<strong>" + user.FullName + "</strong>,</p>\r\n" +
                             "\r\n" +
                             "You can reset your password by clicking <a href=\"" + callbackUrl + "\">this link</a>" +
                             "<p>If you did not request to rest your password kindly ignore this message.\r\n" +
                             "<p>Regards,&nbsp;</p>\r\n" +
                             "<p>National EMR Data Warehouse Access Team</p>";

            await _emailSender.SendEmailAsync(user.Email, subject, message);
            return callbackUrl;
        }

        #endregion Helpers
    }
}
