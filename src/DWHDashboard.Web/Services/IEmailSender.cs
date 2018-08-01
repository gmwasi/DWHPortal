
using System.Threading.Tasks;

namespace DWHDashboard.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}