using Newtonsoft.Json;

namespace DWHDashboard.Web.Models
{
    public class Site
    {
        [JsonProperty("@id")]
        public string Id { get; set; }
        [JsonProperty("@name")]
        public string Name { get; set; }
        [JsonProperty("@contentUrl")]
        public string ContentUrl { get; set; }
    }
}