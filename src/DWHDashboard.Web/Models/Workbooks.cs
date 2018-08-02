using System.Collections.Generic;
using Newtonsoft.Json;

namespace DWHDashboard.Web.Models
{

    public class Workbooks
    {
        [JsonProperty("workbook")]
        public List<Workbook> WorkbookList { get; set; }=new List<Workbook>();
    }
}
