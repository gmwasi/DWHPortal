using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.SharedKernel.Interfaces;

namespace DWHDashboard.ProfileManagement.Core.Interfaces
{
    public interface ITempOrgRepository:IRepository<Organization>
    {
        IEnumerable<TableauView> GetOrgViews(Guid orgId,bool canViewOnly=false);
        IEnumerable<TableauView> GetOrgViewsNoCharts(Guid orgId, bool canViewOnly = false);
        Task<int> UpdateViewsAsync(Guid orgId,IEnumerable<Guid> viewIds);
    }
}