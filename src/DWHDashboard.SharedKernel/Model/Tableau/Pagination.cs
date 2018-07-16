using Newtonsoft.Json;

namespace DWHDashboard.SharedKernel.Model.Tableau
{
    public class Pagination
    {
        [JsonProperty("pageNumber")]
        public string PageNumber { get; set; }

        [JsonProperty("pageSize")]
        public string PageSize { get; set; }

        [JsonProperty("totalAvailable")]
        public string TotalAvailable { get; set; }
    }
}