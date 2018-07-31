using DWHDashboard.SharedKernel.Tests.TestHelpers;
using DWHDashboard.SharedKernel.Data.Repository;

namespace DWHDashboard.SharedKernel.Data.Tests.TestHelpers
{
    public class TestCountyRepository : BaseRepository<TestCounty>
    {
        public TestCountyRepository(TestDbContext context) : base(context)
        {
        }
    }
}