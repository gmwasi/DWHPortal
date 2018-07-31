using DWHDashboard.SharedKernel.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;

namespace DWHDashboard.SharedKernel.Data.Tests.TestHelpers
{
    public class TestDbContext:DbContext
    {
        public DbSet<TestCar> TestCars { get; set; }
        public DbSet<TestCounty> TestCounties { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }
    }
}