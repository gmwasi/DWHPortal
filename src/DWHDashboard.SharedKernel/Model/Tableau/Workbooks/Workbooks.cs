using Newtonsoft.Json;
using System.Collections.Generic;

namespace DWHDashboard.SharedKernel.Model.Tableau.Workbooks
{
    public class Workbooks
    {
        [JsonProperty("workbook")]
        public List<Workbook> WorkbookList { get; set; }
    }
}