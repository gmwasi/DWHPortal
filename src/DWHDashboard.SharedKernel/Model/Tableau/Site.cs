using Newtonsoft.Json;

namespace DWHDashboard.SharedKernel.Model.Tableau
{
    public class Site
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("contentUrl")]
        public string ContentUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public Site()
        {
        }

        private Site(string contentUrl)
        {
            ContentUrl = contentUrl;
        }

        public static Site Create(string contentUrl)
        {
            return new Site(contentUrl);
        }

        public override string ToString()
        {
            return $"{Name} ({ContentUrl})";
        }
    }
}