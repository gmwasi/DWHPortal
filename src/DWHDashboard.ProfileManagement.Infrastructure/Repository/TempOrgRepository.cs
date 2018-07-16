using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DWHDashboard.ProfileManagement.Core.Interfaces;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.ProfileManagement.Infrastructure.Data;
using DWHDashboard.SharedKernel.Data.Repository;

namespace DWHDashboard.ProfileManagement.Infrastructure.Repository
{
    public class TempOrgRepository : BaseRepository<Organization>, ITempOrgRepository
    {
        private readonly ITabViewRepository _tabViewRepository;

        public TempOrgRepository(DwhDashboardContext context, ITabViewRepository tabViewRepository) : base(context)
        {
            _tabViewRepository = tabViewRepository;
        }

        public IEnumerable<TableauView> GetOrgViews(Guid orgId,bool canViewOnly= false)
        {
            var context = Context as DwhDashboardContext;

            var org = Find(orgId);
            var orgViewIds = org.Views.Select(x => x.TabViewId).ToList();

            var allViews = _tabViewRepository.GetViewsFilteredWithCharts().ToList();
             var orgViews=allViews.Where(x => orgViewIds.Contains(x.Id)).ToList();

            if (canViewOnly)
                return orgViews;

            return TableauView.GenerateShowingChecked(allViews, orgViews);
        }

        public IEnumerable<TableauView> GetOrgViewsNoCharts(Guid orgId, bool canViewOnly = false)
        {
            var context = Context as DwhDashboardContext;

            var org = Find(orgId);
            var orgViewIds = org.Views.Select(x => x.TabViewId).ToList();

            var allViews = _tabViewRepository.GetViewsFiltered().ToList();
            var orgViews = allViews.Where(x => orgViewIds.Contains(x.Id)).ToList();

            if (canViewOnly)
                return orgViews;

            return TableauView.GenerateShowingChecked(allViews, orgViews);
        }

        public async Task<int> UpdateViewsAsync(Guid orgId, IEnumerable<Guid> viewIds)
        {
            var context = Context as DwhDashboardContext;
            var orgAccess = context.OrganisationAccesses.Where(x =>x.OrganisationId==orgId).ToList();
            context.OrganisationAccesses.RemoveRange(orgAccess);
            await context.SaveChangesAsync();

            //get all charts ids
            var chartviews=new List<Guid>();
                
//                context.TableauViews
//                .Where(x => x.CustomParentName.ToLower() == "charts".ToLower())                
//                .Select(x => x.Id)
//                .ToList();

            var org = Find(orgId);
            org.UpdateViews(viewIds.Distinct().ToList(), chartviews);
            Update(org);
            
            return await context.SaveChangesAsync();

        }
    }
}