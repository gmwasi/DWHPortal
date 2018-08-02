using System.Collections.Generic;

namespace DWHDashboard.Web.Models
{
    public class WorkbooksResponse
    {
        public Pagination Pagination { get; set; }
        public Workbooks Workbooks { get; set; }
    }
}