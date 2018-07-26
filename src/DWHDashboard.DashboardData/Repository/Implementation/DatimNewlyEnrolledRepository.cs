using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DWHDashboard.DashboardData.Data;
using DWHDashboard.DashboardData.Models;

namespace DWHDashboard.DashboardData.Repository.Implementation
{
    public class DatimNewlyEnrolledRepository : BaseRepository<DatimNewlyEnrolled>, IDatimNewlyEnrolledRepository
    {
        public DatimNewlyEnrolledRepository(DwhDataContext dbContext) : base(dbContext)
        {
        }

        public override Func<string, List<DatimNewlyEnrolled>, List<DatimNewlyEnrolled>> SearchFunc
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
                                n.AgeGroup.ToLower().Contains(st) ||
                                n.FacilityName.ToLower().Contains(st) ||
                                n.County.ToLower().Contains(st) ||
                                n.SubCounty.ToLower().Contains(st) ||
                                n.ImplementingMechnanism.ToLower().Contains(st) ||
                                n.Agency.ToLower().Contains(st) ||
                                n.SiteCode.ToLower().Contains(st))
                            .ToList();
                };
            }
        }
    }
}