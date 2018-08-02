namespace DWHDashboard.Web.Models
{
    public class AuthSession
    {
        public string SiteId { get; set; }
        public string SiteName { get; set; }
        public string Header { get; set; }
        public string Token { get; set; }
        public string Ticket { get; set; }

        public AuthSession()
        {
            Header = "X-Tableau-Auth";
        }
        public AuthSession(SignInResponse signInResponse):this()
        {
            Token = signInResponse.Credentials.Token;
            SiteId = signInResponse.Credentials.Site.Id;
        }

        public override string ToString()
        {
            return $"token:{Token} siteId:{SiteId}";
        }
    }
}