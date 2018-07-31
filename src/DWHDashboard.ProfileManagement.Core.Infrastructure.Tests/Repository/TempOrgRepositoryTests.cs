using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.ProfileManagement.Infrastructure.Data;
using DWHDashboard.ProfileManagement.Infrastructure.Repository;
using DWHDashboard.SharedKernel.Data.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DWHDashboard.ProfileManagement.Core.Infrastructure.Tests.Repository
{
    [TestFixture]
    public class TempOrgRepositoryTests
    {
        private DwhDashboardContext _context;
        private TempOrgRepository _repository;
        private List<Organization> _tempOrgs;
        private List<TableauWorkbook> _workbooks;
        private DbContextOptions<DwhDashboardContext> _contextOptions;

        [SetUp]
        public void SetUp()
        {
            _contextOptions = TestDbOptions.GetInMemoryOptions<DwhDashboardContext>();
            _context = new DwhDashboardContext(_contextOptions);
            _workbooks = TestHelpers.CreateWorkbooks(2, 2);
            TestHelpers.CreateTestData(_context, _workbooks);

            _tempOrgs = TestHelpers.CreateOrgs(1);
            TestHelpers.CreateTestData(_context, _tempOrgs);

            _repository = new TempOrgRepository(_context, new TabViewRepository(_context));
        }

        [Test]
        public void should_Get_Org_Views()
        {
            var tempOrg = _tempOrgs.First();
            var newViews = _workbooks.SelectMany(x => x.TabViews).ToList();
            newViews = newViews.Take(1).ToList();
            var result = _repository.UpdateViewsAsync(tempOrg.Id, newViews.Select(x => x.Id)).Result;

            _repository = new TempOrgRepository(_context, new TabViewRepository(_context));
            var updatedOrgViews = _repository.GetOrgViews(tempOrg.Id).ToList();
            Assert.IsNotEmpty(updatedOrgViews);
            var checkedOnly = updatedOrgViews.Where(x => x.CanView).ToList();
            Assert.IsTrue(checkedOnly.Count == 1);
        }

        [Test]
        public void should_UpdateViewsAsync_With_Org_ViewIds()
        {
            var tempOrg = _tempOrgs.First();
            var newViews = _workbooks.SelectMany(x => x.TabViews).ToList();

            var result = _repository.UpdateViewsAsync(tempOrg.Id, newViews.Select(x => x.Id)).Result;

            _repository = new TempOrgRepository(_context, new TabViewRepository(_context));
            var updatedOrg = _repository.Find(tempOrg.Id);

            Assert.IsNotNull(updatedOrg);
            Assert.IsTrue(updatedOrg.Views.Count == 4);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
            _context = null;
        }
    }
}