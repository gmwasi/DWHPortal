using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.ProfileManagement.Infrastructure.Data;
using DWHDashboard.ProfileManagement.Infrastructure.Repository;
using DWHDashboard.SharedKernel.Data.Tests.TestHelpers;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DWHDashboard.ProfileManagement.Core.Infrastructure.Tests.Repository
{
    [TestFixture]
    public class TabViewRepositoryTests
    {
        private List<TableauView> _tableauViews;
        private DwhDashboardContext _context;
        private TabViewRepository _repository;
        private List<TableauWorkbook> _tableauWorkbookWithViews;
        private List<ViewConfig> _tableauViewConfigs;
        private DbContextOptions<DwhDashboardContext> _contextOptions;

        [SetUp]
        public void SetUp()
        {
            _tableauWorkbookWithViews = TestHelpers.CreateWorkbooks(1, 3);
            _tableauViews = _tableauWorkbookWithViews.SelectMany(x => x.TabViews).ToList();
            _contextOptions = TestDbOptions.GetInMemoryOptions<DwhDashboardContext>();
            _context = new DwhDashboardContext(_contextOptions);
            TestHelpers.CreateTestData(_context, _tableauWorkbookWithViews);
            var configMain = Builder<ViewConfig>.CreateNew().Build();
            configMain.Display = "*";
            configMain.Containing = "live";
            var configAll = Builder<ViewConfig>.CreateNew().Build();
            configAll.Display = "ViewX";
            configAll.Containing = "dashboard";
            TestHelpers.CreateTestData(_context, new List<ViewConfig> { configMain, configAll });
            _repository = new TabViewRepository(_context);
        }

        [Test]
        public void should_Add_or_Update_Views_When_New()
        {
            var workbook = _tableauWorkbookWithViews.First();

            var newViews = Builder<TableauView>.CreateListOfSize(2).Build().ToList();
            foreach (var view in newViews)
            {
                view.TableauId = $"{view.TableauId}-xx";
                view.Name = $"{view.Name}-xx";
                view.Voided = false;
                view.WorkbookTableauId = workbook.TableauId;
                view.TableauWorkbookId = workbook.Id;
                _tableauViews.Add(view);
            }

            var summary = _repository.AddOrUpdateAsync(_tableauViews).Result;

            _repository = new TabViewRepository(_context);
            var updatedViews = _repository.GetAll().ToList();
            var voidedViews = updatedViews.Where(x => x.Voided).ToList();
            Assert.IsTrue(updatedViews.Count == 5);
            Assert.IsTrue(summary.Inserts == 2);
            Assert.IsTrue(summary.Updates == 3);
            Assert.IsTrue(voidedViews.Count == summary.Voids);

            Console.WriteLine(summary.ShowSummary());
        }

        [Test]
        public void should_Add_or_Update_Views_When_Update()
        {
            foreach (var w in _tableauViews)
            {
                w.TableauId = $"{w.TableauId}-s";
                w.Name = $"{w.Name}-s";
                w.Voided = false;
            }
            var summary = _repository.AddOrUpdateAsync(_tableauViews).Result;

            _repository = new TabViewRepository(_context);
            var updatedViews = _repository.GetAll().ToList();
            var voidedViews = updatedViews.Where(x => x.Voided).ToList();
            Assert.IsTrue(updatedViews.Count == 3);
            Assert.IsTrue(summary.Inserts == 0);
            Assert.IsTrue(summary.Updates == 3);
            Assert.IsTrue(voidedViews.Count == summary.Voids);

            Console.WriteLine(summary.ShowSummary());
        }

        [Test]
        public void should_Add_or_Update_Views_When_Voided()
        {
            var toRemove = _tableauViews.Last();
            _tableauViews.Remove(toRemove);

            var summary = _repository.AddOrUpdateAsync(_tableauViews).Result;

            _repository = new TabViewRepository(_context);
            var updatedViews = _repository.GetAll().ToList();
            var voidedViews = updatedViews.Where(x => x.Voided).ToList();
            Assert.IsTrue(voidedViews.Count == 1);
            Assert.IsTrue(updatedViews.Count == 3);
            Assert.IsTrue(summary.Inserts == 0);
            Assert.IsTrue(summary.Updates == 2);
            Assert.IsTrue(summary.Voids == 1);

            Console.WriteLine(summary.ShowSummary());
        }

        [Test]
        public void should_Void_Views()
        {
            var toVoild = new List<TableauView> { _repository.GetAll().First(), _repository.GetAll().Last() };
            _repository.Void(toVoild);
            _repository.Save();

            _repository = new TabViewRepository(_context);
            var voidedList = _repository.GetAll().Where(x => x.Voided).ToList();
            Assert.IsTrue(voidedList.Count == 2);
            Assert.IsNotEmpty(voidedList);

            foreach (var v in voidedList)
            {
                Console.WriteLine($"{v} Voided:{v.Voided}");
            }
        }

        [Test]
        public void should_Add_or_Update_Views_On_second_run()
        {
            var workbook = _tableauWorkbookWithViews.First();

            var newViews = Builder<TableauView>.CreateListOfSize(2).Build().ToList();
            foreach (var view in newViews)
            {
                view.TableauId = $"{view.TableauId}-xx";
                view.Name = $"{view.Name}-xx";
                view.Voided = false;
                view.WorkbookTableauId = workbook.TableauId;
                view.TableauWorkbookId = workbook.Id;
                _tableauViews.Add(view);
            }

            var oldSummary = _repository.AddOrUpdateAsync(_tableauViews).Result;

            var summary = _repository.AddOrUpdateAsync(_tableauViews).Result;

            _repository = new TabViewRepository(_context);
            var updatedViews = _repository.GetAll().ToList();
            var voidedViews = updatedViews.Where(x => x.Voided).ToList();
            Assert.IsTrue(updatedViews.Count == 5);
            Assert.IsTrue(summary.Inserts == 0);
            Assert.IsTrue(summary.Updates == 5);
            Assert.IsTrue(voidedViews.Count == summary.Voids);
            Console.WriteLine(oldSummary);
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(summary.ShowSummary());
        }

        [TestCase("EMR Implementation")]
        [TestCase("GAPR")]
        [TestCase("General")]
        [TestCase("COP")]
        [TestCase("Cohort")]
        [TestCase("Live")]
        [TestCase("Charts")]
        public void should_GetAll_Filtered(string display)
        {
            _tableauWorkbookWithViews = TestHelpers.GetTableauWorkbooks();
            _tableauViews = TestHelpers.GetTableauViews();
            _tableauViewConfigs = TestHelpers.GetViewConfigs();
            _contextOptions = TestDbOptions.GetInMemoryOptions<DwhDashboardContext>();
            _context = new DwhDashboardContext(_contextOptions);

            TestHelpers.CreateTestData(_context, _tableauViewConfigs);
            TestHelpers.CreateTestData(_context, _tableauWorkbookWithViews);
            TestHelpers.CreateTestData(_context, _tableauViews);

            _repository = new TabViewRepository(_context);
            var updates = _repository.UpdateSections().Result;

            _repository = new TabViewRepository(_context);

            var filteredList = _repository.GetViewsFiltered().ToList();
            Assert.IsNotEmpty(filteredList);
            filteredList = filteredList
                .Where(x => x.CustomParentName.ToLower() == display.ToLower())
                .ToList();
            Console.WriteLine(display);
            Console.WriteLine(new string('+', 40));
            int n = 0;
            foreach (var v in filteredList)
            {
                n++;
                Console.WriteLine($"  {n}. {v.Name}");
            }
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string('-', 40));
        }

        [Test]
        public void should_Update_Sections()
        {
            _tableauWorkbookWithViews = TestHelpers.GetTableauWorkbooks();
            _tableauViews = TestHelpers.GetTableauViews();
            _tableauViewConfigs = TestHelpers.GetViewConfigs();
            _contextOptions = TestDbOptions.GetInMemoryOptions<DwhDashboardContext>();
            _context = new DwhDashboardContext(_contextOptions);

            TestHelpers.CreateTestData(_context, _tableauViewConfigs);
            TestHelpers.CreateTestData(_context, _tableauWorkbookWithViews);
            TestHelpers.CreateTestData(_context, _tableauViews);

            _repository = new TabViewRepository(_context);
            var updates = _repository.UpdateSections().Result;

            _repository = new TabViewRepository(_context);
            var filteredList = _repository.GetViewsFiltered()
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.CustomParentName));

            var filteredListRank = _repository.GetViewsFiltered()
                .FirstOrDefault(x => x.Rank.HasValue);

            Assert.IsNotNull(filteredList);
            Console.WriteLine($"{filteredList.Name}|{filteredList.CustomParentName}");

            Assert.IsNotNull(filteredListRank);
            Console.WriteLine($"{filteredListRank.Name}|{filteredListRank.CustomParentName}");
        }

        [Test]
        public void should_Get_ViewsFiltered()
        {
            _tableauWorkbookWithViews = TestHelpers.GetTableauWorkbooks();
            _tableauViews = TestHelpers.GetTableauViews();
            _tableauViewConfigs = TestHelpers.GetViewConfigs();
            _contextOptions = TestDbOptions.GetInMemoryOptions<DwhDashboardContext>();
            _context = new DwhDashboardContext(_contextOptions);

            TestHelpers.CreateTestData(_context, _tableauViewConfigs);
            TestHelpers.CreateTestData(_context, _tableauWorkbookWithViews);
            TestHelpers.CreateTestData(_context, _tableauViews);

            _repository = new TabViewRepository(_context);
            var updates = _repository.UpdateSections().Result;

            _repository = new TabViewRepository(_context);
            var filteredList = _repository.GetViewsFiltered().ToList();
            Assert.IsNotEmpty(filteredList);
            foreach (var v in filteredList)
            {
                Console.WriteLine($"{v.Name}|{v.CustomParentName}");
            }
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