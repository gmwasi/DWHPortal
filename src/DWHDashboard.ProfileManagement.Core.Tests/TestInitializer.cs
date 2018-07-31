using DWHDashboard.DashboardData.Data;
using DWHDashboard.ProfileManagement.Core.Interfaces;
using DWHDashboard.ProfileManagement.Infrastructure.Data;
using DWHDashboard.ProfileManagement.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DWHDashboard.ProfileManagement.Core.Tests
{
    [SetUpFixture]
    public class TestInitializer
    {
        [OneTimeSetUp]
        public void Init()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var serviceProvider = new ServiceCollection()
                .AddDbContext<DwhDashboardContext>(b => b.UseSqlServer(config["ConnectionStrings:DwhDashboardConnection"]))
                .AddTransient<DwhDashboardContext>()
                .AddTransient<DwhDataContext>()
                .AddTransient<IImpersonatorRepository, ImpersonatorRepository>()
                .AddTransient<IOrganizationRepository, OrganizationRepository>()
                .AddTransient<ITabViewRepository, TabViewRepository>()
                .AddTransient<ITabWorkbookRepository, TabWorkbookRepository>()
                .AddTransient<ITempOrgRepository, TempOrgRepository>()
                .AddTransient<IUserRepository, UserRepository>();
        }
    }
}