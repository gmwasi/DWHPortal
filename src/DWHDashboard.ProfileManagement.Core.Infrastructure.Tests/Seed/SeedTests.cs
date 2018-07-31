using System;
using NUnit.Framework;

namespace DWHDashboard.ProfileManagement.Core.Infrastructure.Tests.Seed
{
    [TestFixture]
    public class SeedTests
    {
        private string _resource;

        [SetUp]
        public void SetUp()
        {
            _resource = "TableauUser.csv";
        }

        [Test]
        public void should_readed_emebbeded_csv()
        {
            /*var users = LiveSeeder.ReadCsv("");
            Assert.IsNotEmpty(users);
            foreach (var user in users)
            {
                Console.WriteLine(user.FullName);
            }*/
        }
    }
}