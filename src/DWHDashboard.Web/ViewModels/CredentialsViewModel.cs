using DWHDashboard.Web.ViewModels.Validations;
using FluentValidation.Attributes;
namespace DWHDashboard.Web.ViewModels
{
    [Validator(typeof(CredentialsViewModelValidator))]
    public class CredentialsViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
