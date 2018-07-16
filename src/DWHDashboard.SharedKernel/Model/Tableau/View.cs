using Newtonsoft.Json;

namespace DWHDashboard.SharedKernel.Model.Tableau
{
    public class View
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("contentUrl")]
        public string ContentUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public View()
        {
        }

        private View(string contentUrl)
        {
            ContentUrl = contentUrl;
        }

        public static View Create(string contentUrl)
        {
            return new View(contentUrl);
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}