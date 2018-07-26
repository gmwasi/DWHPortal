using DWHDashboard.DashboardData.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DWHDashboard.DashboardData.Data;

namespace DWHDashboard.DashboardData.Repository.Implementation
{
    public class FacilityRepository : BaseRepository<Facility>, IFacilityRepository
    {
        public FacilityRepository(DwhDataContext dbContext) : base(dbContext)
        {
        }

        public override Func<string, List<Facility>, List<Facility>> SearchFunc
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
                                    n.FacilityCode.ToString().Contains(st) ||
                                    n.FacilityName.ToLower().Contains(st) ||
                                    n.County.ToLower().Contains(st))
                            .ToList();
                };
            }
        }
    }
}