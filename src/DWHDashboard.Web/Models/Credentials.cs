using Newtonsoft.Json;

namespace DWHDashboard.Web.Models
{
    public class Credentials {
    
        public Site Site { get; set; }

        public User User { get; set; }
        [JsonProperty("@name")]
        public string Name { get; set; }

        [JsonProperty("@password")]
        public string Password { get; set; }
        [JsonProperty("@token")]
        public string Token { get; set; }
    }
}