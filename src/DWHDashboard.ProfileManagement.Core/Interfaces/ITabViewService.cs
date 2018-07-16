using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.SharedKernel.Model;

namespace DWHDashboard.ProfileManagement.Core.Interfaces
{
    public interface ITabViewService
    {
        Task<IEnumerable<TableauWorkbook>> GetAllWorkbooksAsync();
        Task<IEnumerable<TableauView>> GetAllViewsAsync(string workbookId);
        Task<SyncDashboardSummary> SyncAsync();

        Task<IEnumerable<TableauView>> GetAllFilteredViewsAsync();
    }
}