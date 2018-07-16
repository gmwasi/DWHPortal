using System.Collections.Generic;
using System.Threading.Tasks;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.SharedKernel.Interfaces;
using DWHDashboard.SharedKernel.Model;

namespace DWHDashboard.ProfileManagement.Core.Interfaces
{
    public interface ITabViewRepository : IRepository<TableauView>
    {
        IEnumerable<TableauView> GetViewsFiltered();
        IEnumerable<TableauView> GetViewsFilteredWithCharts();
        Task<SyncSummary<TableauView>> AddOrUpdateAsync(IEnumerable<TableauView> tableauViews);
        void Void(IEnumerable<TableauView> tableauViews);
        Task<int> UpdateSections();
    }
}