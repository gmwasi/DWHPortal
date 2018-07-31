using DWHDashboard.ProfileManagement.Core.Interfaces;
using DWHDashboard.ProfileManagement.Core.Services;
using DWHDashboard.ProfileManagement.Infrastructure.Data;
using DWHDashboard.ProfileManagement.Infrastructure.Repository;
using DWHDashboard.SharedKernel.Data.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace DWHDashboard.ProfileManagement.Core.Tests.Services
{
    [TestFixture]
    public class TabViewServiceTests
    {
        private readonly string _baseUrl = "http://data.kenyahmis.org/api/2.5/";
        private readonly string _siteName = "NASCOPCOHORT2016";
        private readonly string _user = "KenyaHMIS";
        private string _adminUser;
        private string _adminPass;

        private IAdminService _adminService;
        private ITabViewService _tabViewService;
        private DbContextOptions<DwhDashboardContext> _contextOptions;
        private DwhDashboardContext _context;
        private TabWorkbookRepository _tabWorkbookRepository;
        private TabViewRepository _tabViewRepository;

        [OneTimeSetUp]
        public void Init()
        {
            Factory.Init();
            _contextOptions = TestDbOptions.GetInMemoryOptions<DwhDashboardContext>();
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            var key = File.ReadAllText(TestHelpers.GetKeysFile()).Split('|');

            _adminUser = key[0];
            _adminPass = key[1];

            _contextOptions = TestDbOptions.GetInMemoryOptions<DwhDashboardContext>();
            _context = new DwhDashboardContext(_contextOptions);

            var configs = TestHelpers.GetConfigs();
            TestHelpers.CreateTestData(_context, configs);

            _tabWorkbookRepository = new TabWorkbookRepository(_context);
            _tabViewRepository = new TabViewRepository(_context);

            _adminService = new AdminService(_baseUrl, _adminUser, _adminPass);
            _tabViewService = new TabViewService(_adminService, _baseUrl, _siteName, _tabWorkbookRepository, _tabViewRepository);
        }

        [Test]
        public void should_Get_All_WorkbooksAsync()
        {
            var tabWorkbooks = _tabViewService.GetAllWorkbooksAsync().Result.ToList();
            Assert.IsNotEmpty(tabWorkbooks);
            foreach (var tableauWorkbook in tabWorkbooks)
            {
                Console.WriteLine($"{tableauWorkbook}");
                foreach (var tableauView in tableauWorkbook.TabViews)
                {
                    Assert.AreEqual(tableauWorkbook.TableauId, tableauView.WorkbookTableauId);
                    Console.WriteLine($" >.{tableauView}");
                }
            }
        }

        [Test]
        public void should_Sync_Workbook_Views()
        {
            var summary = _tabViewService.SyncAsync().Result;
            Assert.IsNotNull(summary);
            Console.WriteLine(summary.WorkbookSummary.ShowSummary());
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(summary.ViewSummary.ShowSummary());
        }

        [Test]
        public void should_Sync_Workbook_Views_On_Second_Run()
        {
            var oldSummary = _tabViewService.SyncAsync().Result;

            var summary = _tabViewService.SyncAsync().Result;
            Assert.IsNotNull(summary);
            Console.WriteLine(oldSummary.WorkbookSummary);
            Console.WriteLine(new string('.', 40));
            Console.WriteLine(summary.WorkbookSummary.ShowSummary());
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(oldSummary.ViewSummary);
            Console.WriteLine(new string('.', 40));
            Console.WriteLine(summary.ViewSummary.ShowSummary());
        }

        [TearDown]
        public void TearDown()
        {
            /*_context.Dispose();
            _context = null;*/
        }
    }
}