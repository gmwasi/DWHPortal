using Microsoft.EntityFrameworkCore;

namespace DWHDashboard.SharedKernel.Data.Tests.TestHelpers
{
    public class TestDbOptions
    {
        public static DbContextOptions<T> GetInMemoryOptions<T>(string dbName= "DevDb") where T:DbContext
        {
            var options = new DbContextOptionsBuilder<T>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return options;
        }

        public static DbContextOptions<T> GetOptions<T>(string dbName = "DevDb") where T : DbContext
        {
            return GetInMemoryOptions<T>();
        }
    }
}