using Newtonsoft.Json;

namespace DWHDashboard.SharedKernel.Model.Tableau.SignIn
{
    public class Credentials
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("site")]
        public Site Site { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        public Credentials()
        {
        }

        private Credentials(string name, string password, Site site)
        {
            Name = name;
            Password = password;
            Site = site;
        }

        public static Credentials Create(string name, string password, string siteUrl)
        {
            var site = Tableau.Site.Create(siteUrl);
            return new Credentials(name, password, site);
        }
    }
}