using DWHDashboard.ProfileManagement.Core.Model;
using FizzWare.NBuilder;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DWHDashboard.ProfileManagement.Core.Tests.Model
{
    [TestFixture]
    public class TableauViewTests
    {
        private List<TableauView> _listNew;
        private List<TableauView> _listOld;

        [SetUp]
        public void SetUp()
        {
            _listNew = Builder<TableauView>.CreateListOfSize(5).Build().ToList();
            int n = 0;
            foreach (var a in _listNew)
            {
                n++;
                a.TableauId = n == 3 ? $"{(n + 10)}" : $"{n}";
                a.Name = n == 3 ? $"Name{(n + 10)}" : $"Name{n}";
                a.Voided = false;
            }

            _listOld = Builder<TableauView>.CreateListOfSize(3).Build().ToList();
            n = 0;
            foreach (var b in _listOld)
            {
                n++;
                b.TableauId = $"{n}";
                b.Voided = false;
            }
        }

        [Test]
        public void should_Generate_SyncSummary()
        {
            var summary = TableauView.GenerateSyncSummary(_listOld, _listNew, "Views");
            Assert.AreEqual(3, summary.Inserts);
            Assert.AreEqual(2, summary.Updates);
            Assert.AreEqual(1, summary.Voids);
            Console.WriteLine(summary.ShowSummary());
        }
    }
}