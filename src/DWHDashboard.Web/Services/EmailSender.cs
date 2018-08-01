using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DWHDashboard.Web.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _host;
        private readonly int _port;
        private readonly bool _enableSsl;
        private readonly string _userName;
        private readonly string _password;

        public EmailSender(string host, int port, bool enableSsl, string userName, string password)
        {
            _host = host;
            _port = port;
            _enableSsl = enableSsl;
            _userName = userName;
            _password = password;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_userName, _password),
                EnableSsl = _enableSsl
            };
            var fullSubject = $"Data warehouse Access (do not reply) - {subject}";
            return client.SendMailAsync(new MailMessage(_userName, email, fullSubject, htmlMessage) { IsBodyHtml = true }
            );
        }
    }
}