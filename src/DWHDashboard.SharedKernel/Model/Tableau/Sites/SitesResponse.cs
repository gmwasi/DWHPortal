using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace DWHDashboard.SharedKernel.Model.Tableau.Sites
{
    public class SitesResponse
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("sites")]
        public Sites Sites { get; set; }

        public List<Site> GetSites()
        {
            return Sites.SiteList;
        }

        public string GetSiteId(string siteName)
        {
            var site = GetSites()?.FirstOrDefault(x => x.Name.ToLower() == siteName.ToLower());

            if (site != null)
                return site.Id;

            return string.Empty;
        }
    }
}