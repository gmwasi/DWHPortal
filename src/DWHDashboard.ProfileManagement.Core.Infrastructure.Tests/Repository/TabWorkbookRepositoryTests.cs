using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.ProfileManagement.Infrastructure.Data;
using DWHDashboard.ProfileManagement.Infrastructure.Repository;
using DWHDashboard.SharedKernel.Data.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DWHDashboard.ProfileManagement.Core.Infrastructure.Tests.Repository
{
    [TestFixture]
    public class TabWorkbookRepositoryTests
    {
        private List<TableauWorkbook> _tableauWorkbookWithViews;
        private DbContextOptions<DwhDashboardContext> _contextOptions;
        private DwhDashboardContext _context;
        private TabWorkbookRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _tableauWorkbookWithViews = TestHelpers.CreateWorkbooks(3, 2);
            _contextOptions = TestDbOptions.GetInMemoryOptions<DwhDashboardContext>();
            _context = new DwhDashboardContext(_contextOptions);
            TestHelpers.CreateTestData(_context, _tableauWorkbookWithViews);
            _repository = new TabWorkbookRepository(_context);
        }

        [Test]
        public void should_Add_or_Update_Workbooks_When_New()
        {
            var newWorkbooks = TestHelpers.CreateWorkbooks(2, 1);
            foreach (var w in newWorkbooks)
            {
                w.TableauId = $"{w.TableauId}-xx";
                w.Name = $"{w.Name}-xx";
                w.Voided = false;
                _tableauWorkbookWithViews.Add(w);
            }

            var summary = _repository.AddOrUpdateAsync(_tableauWorkbookWithViews).Result;

            _repository = new TabWorkbookRepository(_context);
            var updatedWorkbooks = _repository.GetAll().ToList();
            var voidedWorkbooks = updatedWorkbooks.Where(x => x.Voided).ToList();
            Assert.IsTrue(updatedWorkbooks.Count == 5);
            Assert.IsTrue(summary.Inserts == 2);
            Assert.IsTrue(summary.Updates == 3);
            Assert.IsTrue(voidedWorkbooks.Count == summary.Voids);

            Console.WriteLine(summary.ShowSummary());
        }

        [Test]
        public void should_Add_or_Update_Workbooks_When_Update()
        {
            foreach (var w in _tableauWorkbookWithViews)
            {
                w.TableauId = $"{w.TableauId}-s";
                w.Name = $"{w.Name}-s";
                w.Voided = false;
            }
            var summary = _repository.AddOrUpdateAsync(_tableauWorkbookWithViews).Result;

            _repository = new TabWorkbookRepository(_context);
            var updatedWorkbooks = _repository.GetAll().ToList();
            var voidedWorkbooks = updatedWorkbooks.Where(x => x.Voided).ToList();
            Assert.IsTrue(updatedWorkbooks.Count == 3);
            Assert.IsTrue(summary.Inserts == 0);
            Assert.IsTrue(summary.Updates == 3);
            Assert.IsTrue(voidedWorkbooks.Count == summary.Voids);

            Console.WriteLine(summary.ShowSummary());
        }

        [Test]
        public void should_Add_or_Update_Workbooks_When_Voided()
        {
            var toRemove = _tableauWorkbookWithViews.Last();
            _tableauWorkbookWithViews.Remove(toRemove);

            var summary = _repository.AddOrUpdateAsync(_tableauWorkbookWithViews).Result;

            _repository = new TabWorkbookRepository(_context);
            var updatedWorkbooks = _repository.GetAll().ToList();
            var voidedWorkbooks = updatedWorkbooks.Where(x => x.Voided).ToList();
            Assert.IsTrue(voidedWorkbooks.Count == 1);
            Assert.IsTrue(updatedWorkbooks.Count == 3);
            Assert.IsTrue(summary.Inserts == 0);
            Assert.IsTrue(summary.Updates == 2);
            Assert.IsTrue(summary.Voids == 1);

            Console.WriteLine(summary.ShowSummary());
        }

        [Test]
        public void should_Void_Workbooks()
        {
            var toVoild = new List<TableauWorkbook> { _repository.GetAll().First(), _repository.GetAll().Last() };
            _repository.Void(toVoild);
            _repository.Save();

            _repository = new TabWorkbookRepository(_context);
            var voidedList = _repository.GetAll().Where(x => x.Voided).ToList();
            Assert.IsTrue(voidedList.Count == 2);
            Assert.IsNotEmpty(voidedList);

            foreach (var v in voidedList)
            {
                Console.WriteLine($"{v} Voided:{v.Voided}");
            }
        }

        [Test]
        public void should_Add_or_Update_Workbooks_On_SecondRun()
        {
            var newWorkbooks = TestHelpers.CreateWorkbooks(2, 1);
            foreach (var w in newWorkbooks)
            {
                w.TableauId = $"{w.TableauId}-xx";
                w.Name = $"{w.Name}-xx";
                w.Voided = false;
                _tableauWorkbookWithViews.Add(w);
            }
            var oldSummary = _repository.AddOrUpdateAsync(_tableauWorkbookWithViews).Result;

            var summary = _repository.AddOrUpdateAsync(_tableauWorkbookWithViews).Result;

            _repository = new TabWorkbookRepository(_context);
            var updatedWorkbooks = _repository.GetAll().ToList();
            var voidedWorkbooks = updatedWorkbooks.Where(x => x.Voided).ToList();
            Assert.IsTrue(updatedWorkbooks.Count == 5);
            Assert.IsTrue(summary.Inserts == 0);
            Assert.IsTrue(summary.Updates == 5);
            Assert.IsTrue(voidedWorkbooks.Count == summary.Voids);
            Console.WriteLine(oldSummary);
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(summary.ShowSummary());
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _context = null;
        }
    }
}