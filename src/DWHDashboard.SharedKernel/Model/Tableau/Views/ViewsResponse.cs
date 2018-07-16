using Newtonsoft.Json;
using System.Collections.Generic;

namespace DWHDashboard.SharedKernel.Model.Tableau.Views
{
    public class ViewsResponse
    {
        [JsonProperty("views")]
        public Views Views { get; set; }

        public List<View> GetViews()
        {
            return Views.ViewsList;
        }
    }
}