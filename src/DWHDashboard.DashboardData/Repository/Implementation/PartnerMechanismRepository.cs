using DWHDashboard.DashboardData.Data;
using DWHDashboard.DashboardData.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DWHDashboard.DashboardData.Repository.Implementation
{
    public class PartnerMechanismRepository : BaseRepository<PartnerMechanism>, IPartnerMechanismRepository
    {
        public PartnerMechanismRepository(DwhDataContext dbContext) : base(dbContext)
        {
        }

        public override Func<string, List<PartnerMechanism>, List<PartnerMechanism>> SearchFunc
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
                                    n.MFL_Code.ToString().Contains(st) ||
                                    n.FacilityName.ToLower().Contains(st) ||
                                    n.Mechanism.ToLower().Contains(st) ||
                                    n.County.ToLower().Contains(st))
                            .ToList();
                };
            }
        }
    }
}