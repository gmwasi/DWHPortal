
// <pagination pageNumber="1" pageSize="100" totalAvailable="1" />

using Newtonsoft.Json;

namespace DWHDashboard.Web.Models
{
    public class Pagination {
        public string PageNumber { get; set; }
        public string PageSize { get; set; }
        public string TotalAvailable { get; set; }
    }
}