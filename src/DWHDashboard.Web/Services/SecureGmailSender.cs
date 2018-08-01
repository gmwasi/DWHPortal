using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

namespace DWHDashboard.Web.Services
{
    public class SecureGmailSender : IEmailSender
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _userName;
        private readonly string _authorizationCode;

        public SecureGmailSender(string host, int port, string userName, string authorizationCode)
        {
            _host = host;
            _port = port;
            _userName = userName;
            _authorizationCode = authorizationCode;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fullSubject = $"Data warehouse Access (do not reply) - {subject}";
            var accessToken = GetAccessToken();
            var message = new MailMessage(_userName, email, fullSubject, htmlMessage) {IsBodyHtml = true};
            var client = new SmtpClient(_host, _port, _userName, accessToken, true)
            {
                SecurityOptions = SecurityOptions.SSLImplicit
            };
            client.Timeout = 400000;
            return client.BeginSend(message) as Task;
        }

        internal string GetAccessToken()
        {
            string actionUrl = "https://accounts.google.com/o/oauth2/token";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(actionUrl);
            request.CookieContainer = new CookieContainer();
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            string encodedParameters = string.Format(
                "client_id={1}&code={0}&client_secret={2}&redirect_uri={3}&grant_type={4}",
                HttpUtility.UrlEncode(_authorizationCode),
                HttpUtility.UrlEncode("929645059575.apps.googleusercontent.com"),
                HttpUtility.UrlEncode("USnH5eQRsC4XrjJbpGG7WVq5"),
                HttpUtility.UrlEncode("urn:ietf:wg:oauth:2.0:oob"),
                HttpUtility.UrlEncode("authorization_code")
            );
            byte[] requestData = Encoding.UTF8.GetBytes(encodedParameters);
            request.ContentLength = requestData.Length;
            if (requestData.Length > 0)
                using (Stream stream = request.GetRequestStream())
                    stream.Write(requestData, 0, requestData.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseText = null;
            using (TextReader reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
                responseText = reader.ReadToEnd();
            string accessToken = null;
            foreach (string sPair in responseText.
                Replace("{", "").
                Replace("}", "").
                Replace("\"", "").
                Split(new string[] { ",\n" }, StringSplitOptions.None))
            {
                string[] pair = sPair.Split(':');
                if ("access_token" == pair[0].Trim())
                {
                    accessToken = pair[1].Trim();
                    break;
                }
            }
            return accessToken;
        }
    }
}