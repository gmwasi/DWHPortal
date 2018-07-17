using DWHDashboard.ProfileManagement.Core.Interfaces;
using DWHDashboard.ProfileManagement.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DWHDashboard.ProfileManagement.Core.Services
{
    public class LookupService : ILookupService
    {
        private readonly IOrganizationRepository _OrganizationRepository;

        public LookupService(IOrganizationRepository organizationRepository)
        {
            _OrganizationRepository = organizationRepository;
        }

        public async Task<List<Organization>> GetOrganizations()
        {
            var organizations = _OrganizationRepository.GetAll().ToList();
            return organizations;
        }
    }
}