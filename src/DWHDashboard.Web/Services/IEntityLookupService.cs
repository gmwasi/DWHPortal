using System.Collections.Generic;
using System.Threading.Tasks;

namespace DWHDashboard.Web.Services
{
    public interface IEntityLookupService
    {
        Task<List<string>> GetCounties();

        Task<List<string>> GetFacilities();

        Task<List<string>> GetPartners();
    }
}