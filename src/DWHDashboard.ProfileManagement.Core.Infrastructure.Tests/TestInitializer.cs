using System;
using DWHDashboard.ProfileManagement.Core.Interfaces;
using DWHDashboard.ProfileManagement.Infrastructure.Data;
using DWHDashboard.ProfileManagement.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DWHDashboard.ProfileManagement.Core.Infrastructure.Tests
{
    [SetUpFixture]
    public class TestInitializer
    {
        public static IServiceProvider ServiceProvider;
        [OneTimeSetUp]
        public void Init()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var serviceProvider = new ServiceCollection()
                .AddDbContext<DwhDashboardContext>(b => b.UseSqlServer(config["ConnectionStrings:DwhDashboardConnection"]))
                .AddTransient<DwhDashboardContext>()
                .AddTransient<IImpersonatorRepository, ImpersonatorRepository>()
                .AddTransient<IOrganizationRepository, OrganizationRepository>()
                .AddTransient<ITabViewRepository, TabViewRepository>()
                .AddTransient<ITabWorkbookRepository, TabWorkbookRepository>()
                .AddTransient<ITempOrgRepository, TempOrgRepository>()
                .AddTransient<IUserRepository, UserRepository>()
                .BuildServiceProvider();

            ServiceProvider = serviceProvider;

            var dwhDashboardContext = serviceProvider.GetService<DwhDashboardContext>();

            dwhDashboardContext.Database.Migrate();
            //dwhDashboardContext.EnsureSeeded();
        }
    }
}