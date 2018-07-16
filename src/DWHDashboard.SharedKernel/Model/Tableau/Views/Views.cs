using Newtonsoft.Json;
using System.Collections.Generic;

namespace DWHDashboard.SharedKernel.Model.Tableau.Views
{
    public class Views
    {
        [JsonProperty("view")]
        public List<View> ViewsList { get; set; }
    }
}