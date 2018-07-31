using System;
using DWHDashboard.SharedKernel.Tests.TestHelpers;
using DWHDashboard.SharedKernel.Data.Repository;

namespace DWHDashboard.SharedKernel.Data.Tests.TestHelpers
{
    public class TestCarRepository : BaseRepository<TestCar>
    {
        public TestCarRepository(TestDbContext context) : base(context)
        {
        }
    }
}