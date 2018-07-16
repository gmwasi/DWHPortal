using Newtonsoft.Json;

namespace DWHDashboard.SharedKernel.Model.Tableau
{
    public class Workbook
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("contentUrl")]
        public string ContentUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public Workbook()
        {
        }

        private Workbook(string contentUrl)
        {
            ContentUrl = contentUrl;
        }

        public static Workbook Create(string contentUrl)
        {
            return new Workbook(contentUrl);
        }

        public override string ToString()
        {
            return $"{Name} ({ContentUrl})";
        }
    }
}