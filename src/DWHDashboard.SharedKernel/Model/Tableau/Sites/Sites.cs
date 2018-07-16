using Newtonsoft.Json;
using System.Collections.Generic;

namespace DWHDashboard.SharedKernel.Model.Tableau.Sites
{
    public class Sites
    {
        [JsonProperty("site")]
        public List<Site> SiteList { get; set; }
    }
}