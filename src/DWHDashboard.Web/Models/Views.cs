using System.Collections.Generic;
using Newtonsoft.Json;

namespace DWHDashboard.Web.Models
{

    public class Views
    {
        [JsonProperty("view")]
        public List<View> ViewList { get; set; }=new List<View>();
    }
}
