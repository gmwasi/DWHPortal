using FluentValidation;

namespace DWHDashboard.Web.ViewModels.Validations
{
    public class RegistrationViewModelValidator : AbstractValidator<RegistrationViewModel>
    {
        public RegistrationViewModelValidator()
        {
            RuleFor(vm => vm.Email).NotEmpty().WithMessage("Email cannot be empty");
            RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password cannot be empty");
            RuleFor(vm => vm.FullName).NotEmpty().WithMessage("FirstName cannot be empty");
            RuleFor(vm => vm.UserName).NotEmpty().WithMessage("LastName cannot be empty");
        }
    }
}
