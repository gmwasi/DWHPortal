using DWHDashboard.DashboardData.Data;
using DWHDashboard.DashboardData.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DWHDashboard.DashboardData.Repository.Implementation
{
    public class ViralLoadMatrixRepository : BaseRepository<ViralLoadMatrix>, IViralLoadMatrixRepository
    {
        public ViralLoadMatrixRepository(DwhDataContext dbContext) : base(dbContext)
        {
        }

        public override Func<string, List<ViralLoadMatrix>, List<ViralLoadMatrix>> SearchFunc
        {
            get
            {
                return (searchText, allItems) =>
                {
                    if (string.IsNullOrEmpty(searchText)) return allItems;
                    var st = searchText.ToLower();

                    return
                        allItems.Where(
                                n =>
                                n.TestName.ToLower().Contains(st) ||
                                n.TestResult.ToLower().Contains(st) ||
                                n.PatientId.ToLower().Contains(st) ||
                                n.PatientPk.ToLower().Contains(st) ||
                                n.Project.ToLower().Contains(st) ||
                                n.SiteCode.ToLower().Contains(st))
                            .ToList();
                };
            }
        }
    }
}