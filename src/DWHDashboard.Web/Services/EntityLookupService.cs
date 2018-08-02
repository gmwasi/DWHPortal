using DWHDashboard.DashboardData.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DWHDashboard.Web.Services
{
    public class EntityLookupService : IEntityLookupService
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IPartnerMechanismRepository _partnerMechanismRepository;

        public EntityLookupService(IFacilityRepository facilityRepository, IPartnerMechanismRepository partnerMechanismRepository)
        {
            _facilityRepository = facilityRepository;
            _partnerMechanismRepository = partnerMechanismRepository;
        }

        public async Task<List<string>> GetCounties()
        {
            return await Task.Run(() => _facilityRepository.GetAll().OrderBy(n => n.County).Select(i => i.County).Distinct().ToList());
        }

        public async Task<List<string>> GetFacilities()
        {
            return await Task.Run(() => _facilityRepository.GetAll().OrderBy(n => n.FacilityName).Select(i => i.FacilityName).Distinct().ToList());
        }

        public async Task<List<string>> GetPartners()
        {
            return await Task.Run(() => _partnerMechanismRepository.GetAll().OrderBy(n => n.Mechanism).Select(i => i.Mechanism).Distinct().ToList());
        }
    }
}