using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DWHDashboard.ProfileManagement.Core.Interfaces;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.ProfileManagement.Infrastructure.Data;
using DWHDashboard.SharedKernel.Data.Repository;
using DWHDashboard.SharedKernel.Model;

namespace DWHDashboard.ProfileManagement.Infrastructure.Repository
{
    public class TabViewRepository : BaseRepository<TableauView>, ITabViewRepository
    {
        public TabViewRepository(DwhDashboardContext context) : base(context)
        {
        }

        public IEnumerable<TableauView> GetViewsFiltered()
        {
            var viewsFiltered = GetAll()
                .Where(x => !string.IsNullOrWhiteSpace(x.CustomParentName) &&
                            x.CustomParentName.ToLower() == "live".ToLower());

            viewsFiltered = viewsFiltered.OrderBy(x => x.Rank).ToList();

            return viewsFiltered;
        }

        public IEnumerable<TableauView> GetViewsFilteredWithCharts()
        {
            var viewsFiltered = GetAll()
                .Where(x => !string.IsNullOrWhiteSpace(x.CustomParentName));

            viewsFiltered = viewsFiltered.OrderBy(x => x.Rank).ToList();

            return viewsFiltered;
        }

        public async Task<int> UpdateSections()
        {
            var viewsFiltered = GetAll().ToList();

            var viewConfigs = ((DwhDashboardContext) Context).ViewConfigs.ToList();
            var otherViewConfigs = viewConfigs.Where(x => x.Display != "*").ToList();
            var filterdList = new List<TableauView>();

            #region Others

            foreach (var config in otherViewConfigs)
            {
                var otherViewsFiltered = viewsFiltered;

                #region Others.LIKE

                if (!string.IsNullOrWhiteSpace(config.Containing))
                {
                    string[] filters = config.Containing.Split('^');

                    if (filters.Length > 0)
                    {
                        foreach (var filter in filters)
                        {
                            otherViewsFiltered = otherViewsFiltered
                                .Where(x => x.Name.ToLower().Contains(filter.ToLower()))
                                .ToList();
                        }
                        foreach (var o in otherViewsFiltered)
                        {
                            o.CustomParentName = config.Display;
                            o.Rank = config.Rank;
                        }
                    }
                }

                #endregion

                #region Others.NOT LIKE

                if (!string.IsNullOrWhiteSpace(config.NotContaining))
                {
                    string[] excludeFilters = config.NotContaining.Split('^');

                    if (excludeFilters.Length > 0)
                    {
                        foreach (var excludeFilter in excludeFilters)
                        {
                            otherViewsFiltered = otherViewsFiltered
                                .Where(x => !x.Name.ToLower().Contains(excludeFilter.ToLower()))
                                .ToList();
                        }
                        foreach (var o in otherViewsFiltered)
                        {

                            o.CustomParentName = config.Display;
                            o.Rank = config.Rank;
                        }
                    }
                }

                #endregion

                if (otherViewsFiltered.Count > 0)
                    filterdList.AddRange(otherViewsFiltered);
            }

            #endregion

            filterdList = filterdList.OrderBy(x => x.Rank).ToList();


            Update(filterdList);

            return await SaveAsync();
        }

        public async Task<SyncSummary<TableauView>> AddOrUpdateAsync(IEnumerable<TableauView> tableauViews)
        {
            var exisitngViews = GetAll().ToList();
            var summary = TableauView.GenerateSyncSummary(exisitngViews, tableauViews, "Views");


            var insertList = summary.InsertList;

            if (insertList.Count > 0)
            {
                var workbookTabIds = insertList.Select(x => x.WorkbookTableauId).Distinct();
                var context = Context as DwhDashboardContext;
                var workbooks = context.TableauWorkbooks.Where(x => workbookTabIds.Contains(x.TableauId)).Select(
                    x => new
                    {
                        Rid = x.Id,
                        Tid = x.TableauId
                    }).ToList();

                foreach (var i in insertList)
                {
                    var exisitingWorkbook =
                        workbooks.FirstOrDefault(x => x.Tid.ToLower() == i.WorkbookTableauId.ToLower());
                    if (null != exisitingWorkbook)
                        i.TableauWorkbookId = exisitingWorkbook.Rid;
                }
            }

            Create(insertList);

            await SaveAsync();

            Update(summary.UpdateList);
            Void(summary.VoidsList);

            await SaveAsync();

            return summary;
        }

        public void Void(IEnumerable<TableauView> tableauViews)
        {
            var voidList = tableauViews.ToList();

            foreach (var t in voidList)
            {
                t.Voided = true;
            }
            Update(voidList);
        }
    }
}