using System;
using System.Collections.Generic;
using System.Linq;
using DWHDashboard.SharedKernel.Model;
using FizzWare.NBuilder;
using NUnit.Framework;

namespace DWHDashboard.SharedKernel.Tests.Utility
{
    [TestFixture]
    public class TableauEntityComparerTests
    {
        private List<TableauTestView> _listNew, _listOld;
        [SetUp]
        public void Setup()
        {
             _listNew = Builder<TableauTestView>.CreateListOfSize(3).Build().ToList();
            int n = 0;
            foreach (var a in _listNew)
            {
                n++;
                a.TableauId = $"{n}";
                a.Voided = false;
            }

            _listOld = Builder<TableauTestView>.CreateListOfSize(2).Build().ToList();
            n = 0;
            foreach (var b in _listOld)
            {
                n++;
                b.TableauId = $"{n}";
                b.Voided = false;
            }
        }

        [Test]
        public void should_Get_SyncSummary()
        {
            var sumamry = TableauTestView.GenerateSyncSummary(_listOld, _listNew,"Test");
            Assert.IsNotNull(sumamry);
            Console.WriteLine(sumamry.ShowSummary());
        }

        [Test]
        public void should_Get_New_Lists()
        {
            var sumamry = TableauTestView.GenerateSyncSummary(_listOld, _listNew, "Test");
            var newOnly = sumamry.InsertList;
            Assert.IsNotEmpty(newOnly);
            var newItem = newOnly.First();
            Console.WriteLine(sumamry.ShowSummary());
        }

        [Test]
        public void should_Get_Updated_Lists()
        {
            var sumamry = TableauTestView.GenerateSyncSummary(_listOld, _listNew, "Test");
            var updatedOnly = sumamry.UpdateList;
            Assert.IsNotEmpty(updatedOnly);
            Assert.IsTrue(updatedOnly.Count == 2);
            Console.WriteLine(sumamry.ShowSummary());
        }

        [Test]
        public void should_Get_Voided_Lists()
        {
            var toRemove = _listNew.First(x => x.TableauId == "2");
            _listNew.Remove(toRemove);
            
            var sumamry = TableauTestView.GenerateSyncSummary(_listOld, _listNew, "Test");
            var voidedOnly = sumamry.VoidsList;
            Assert.IsNotEmpty(voidedOnly);
            Assert.IsTrue(voidedOnly.Count == 1);
            var voidedItem = voidedOnly.First();
            Assert.AreEqual("2",voidedItem.TableauId);
            
            Console.WriteLine(sumamry.ShowSummary());
        }
    }

    public class TableauTestView :TableauEntity
    {
        public override string ToString()
        {
            return $"{TableauId} | {Name} |{Voided}";
        }
    }
}