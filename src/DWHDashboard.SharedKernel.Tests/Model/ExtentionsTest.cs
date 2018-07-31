using System;
using DWHDashboard.SharedKernel.Tests.TestHelpers;
using DWHDashboard.SharedKernel.Utility;
using NUnit.Framework;

namespace DWHDashboard.SharedKernel.Tests.Model
{
    
    [TestFixture]
    public class EntityTests
    {
        [Test]
        public void should_have_guid_value()
        {
           var t=new TestEntity("Subaru");
            Assert.False(t.Id.IsNullOrEmpty());
            Console.WriteLine(t);
        }

        [Test]
        public void should_have_assigned_id_value()
        {
            var id = LiveGuid.NewGuid();
            var t = new TestEntity(id, "Subaru");
            Assert.AreEqual(id,t.Id);
            Console.WriteLine(t);
        }
    }
}