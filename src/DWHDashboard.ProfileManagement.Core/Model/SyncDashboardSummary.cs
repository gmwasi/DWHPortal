using DWHDashboard.SharedKernel.Model;

namespace DWHDashboard.ProfileManagement.Core.Model
{
    public class SyncDashboardSummary
    {
        public SyncSummary<TableauWorkbook> WorkbookSummary { get; set; }
        public SyncSummary<TableauView> ViewSummary { get; set; }
    }
}