using System;
using DWHDashboard.SharedKernel.Utility;
using NUnit.Framework;

namespace DWHDashboard.SharedKernel.Tests.Utility
{
    [TestFixture]
    public class ExtentionsTests
    {
        [Test]
        public void should_check_null_guids()
        {
           

             var blank2=new Guid();
            Assert.IsTrue(blank2.IsNullOrEmpty());

            var blank3 = Guid.Empty;
            Assert.IsTrue(blank3.IsNullOrEmpty());

            var blank4 = LiveGuid.NewGuid();
            Assert.IsFalse(blank4.IsNullOrEmpty());

            Guid? blank5=null;
            Assert.IsTrue(blank5.IsNullOrEmpty());

            blank5 = new Guid();
            Assert.IsTrue(blank5.IsNullOrEmpty());

            blank5 = Guid.Empty;
            Assert.IsTrue(blank5.IsNullOrEmpty());

            blank5 = LiveGuid.NewGuid();
            Assert.IsFalse(blank5.IsNullOrEmpty());
        }
        
    }
}