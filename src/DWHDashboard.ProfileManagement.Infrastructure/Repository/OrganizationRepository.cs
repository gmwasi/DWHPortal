using DWHDashboard.ProfileManagement.Core.Interfaces;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.SharedKernel.Data.Repository;
using DWHDashboard.ProfileManagement.Infrastructure.Data;

namespace DWHDashboard.ProfileManagement.Infrastructure.Repository
{
    public class OrganizationRepository : BaseRepository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(DwhDashboardContext context) : base(context)
        {
        }
    }
}