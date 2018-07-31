using DWHDashboard.ProfileManagement.Core.Interfaces;
using DWHDashboard.ProfileManagement.Core.Services;
using NUnit.Framework;
using System;
using System.IO;

namespace DWHDashboard.ProfileManagement.Core.Tests.Services
{
    [TestFixture]
    public class AdminServiceTests
    {
        private readonly string _baseUrl = "http://data.kenyahmis.org/api/2.5/";
        private readonly string _siteName = "NASCOPCOHORT2016";
        private readonly string _user = "KenyaHMIS";
        private string _adminUser;
        private string _adminPass;

        private IAdminService _adminService;

        [SetUp]
        public void SetUp()
        {
            var key = File.ReadAllText(TestHelpers.GetKeysFile()).Split('|');

            _adminUser = key[0];
            _adminPass = key[1];

            _adminService = new AdminService(_baseUrl, _adminUser, _adminPass);
        }

        [Test]
        public void should_Get_AdminAuthToken()
        {
            var signInResponse = _adminService.GetAdminAuthTokenAsync().Result;
            Assert.IsNotNull(signInResponse);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(signInResponse.Credentials.Token));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(signInResponse.Credentials.User.Id));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(signInResponse.Credentials.Site.Id));
            Console.WriteLine(signInResponse);
        }

        [Test]
        public void should_Get_AllSites()
        {
            var signInResponse = _adminService.GetAdminAuthTokenAsync().Result;
            Assert.IsNotNull(signInResponse);

            var sitesResponse = _adminService.GetAllSitesAsync().Result;
            Assert.IsNotNull(sitesResponse);
            var sites = sitesResponse.GetSites();
            Assert.IsNotEmpty(sites);

            Console.WriteLine(new string('-', 40));
            foreach (var s in sites)
            {
                Assert.IsTrue(!string.IsNullOrWhiteSpace(s.Id));
                Console.WriteLine($" > {s} [{s.Id}]");
            }
            Console.WriteLine(new string('-', 40));
        }

        [Test]
        public void should_Get_Site_Users()
        {
            var signInResponse = _adminService.GetAdminAuthTokenAsync().Result;
            Assert.IsNotNull(signInResponse);

            var siteUsersResponse = _adminService.GetSiteUsersAsync(_siteName).Result;
            Assert.IsNotNull(siteUsersResponse);
            var users = siteUsersResponse.GetUsers();
            Assert.IsNotEmpty(users);

            Console.WriteLine(new string('-', 40));
            foreach (var s in users)
            {
                Assert.IsTrue(!string.IsNullOrWhiteSpace(s.Id));
                Console.WriteLine($" > {s} [{s.Id}]");
            }
            Console.WriteLine(new string('-', 40));
        }

        [Test]
        public void should_Find_User_In_Site()
        {
            var signInUser = _adminService.FindUser(_user, _siteName).Result;
            Assert.IsNotNull(signInUser);

            Console.WriteLine(signInUser);
        }
    }
}