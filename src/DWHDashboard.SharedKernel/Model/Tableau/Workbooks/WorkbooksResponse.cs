using Newtonsoft.Json;
using System.Collections.Generic;

namespace DWHDashboard.SharedKernel.Model.Tableau.Workbooks
{
    public class WorkbooksResponse
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("workbooks")]
        public Workbooks Workbooks { get; set; }

        public List<Workbook> GetWorkbooks()
        {
            return Workbooks.WorkbookList;
        }
    }
}