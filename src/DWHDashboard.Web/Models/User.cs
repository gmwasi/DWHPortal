using Newtonsoft.Json;

namespace DWHDashboard.Web.Models
{
    public class User
    {
        [JsonProperty("@id")]
        public string Id { get; set; }
    }
}