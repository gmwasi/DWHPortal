using DWHDashboard.ProfileManagement.Core.Interfaces;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.ProfileManagement.Infrastructure.Data;
using DWHDashboard.SharedKernel.Data.Repository;

namespace DWHDashboard.ProfileManagement.Infrastructure.Repository
{
    public class ImpersonatorRepository: BaseRepository<Impersonator>, IImpersonatorRepository
    {
        public ImpersonatorRepository(DwhDashboardContext context) : base(context)
        {
        }
    }
}