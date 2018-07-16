using Newtonsoft.Json;

namespace DWHDashboard.SharedKernel.Model.Tableau.SignIn
{
    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("siteRole")]
        public string SiteRole { get; set; }

        [JsonProperty("lastLogin")]
        public string LastLogin { get; set; }

        public override string ToString()
        {
            return $"{Name} ({FullName})";
        }
    }
}