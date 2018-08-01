using DWHDashboard.ProfileManagement.Core.Interfaces;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.ProfileManagement.Infrastructure.Data;
using DWHDashboard.ProfileManagement.Infrastructure.Repository;
using DWHDashboard.SharedKernel.Data.Tests.TestHelpers;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace DWHDashboard.ProfileManagement.Core.Infrastructure.Tests.Repository
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private DbConnection _connection;
        private DwhDashboardContext _context;
        private DbContextOptions<DwhDashboardContext> _contextOptions;
        private IUserRepository _repository;
        private List<Organization> _organizations;
        private List<User> _users;

        [SetUp]
        public void SetUp()
        {
            _contextOptions = TestDbOptions.GetInMemoryOptions<DwhDashboardContext>();
            _context = new DwhDashboardContext(_contextOptions);
            _organizations = TestHelpers.CreateOrgs(2);
            TestHelpers.CreateTestData(_context, _organizations);
            var impersonators = Builder<Impersonator>.CreateListOfSize(1).Build();
            TestHelpers.CreateTestData(_context, impersonators);

            _users = new List<User>();
            foreach (var o in _organizations)
            {
                var id = impersonators.First().Id;
                _users.AddRange(TestHelpers.CreateUsers(1, UserType.Steward, o.Id, id));
                _users.AddRange(TestHelpers.CreateUsers(3, UserType.Guest, o.Id, id));
                _users.AddRange(TestHelpers.CreateUsers(2, UserType.Normal, o.Id, id));
                _users.Where(x => x.UserType == UserType.Guest).First().EmailConfirmed = false;
            }

            TestHelpers.CreateTestData(_context, _users);
            _repository = new UserRepository(_context);
        }

        [Test]
        public void should_Update_User_Profile()
        {
            var userToUpdate = _repository.GetAllUsers().FirstOrDefault();
            Assert.IsNotNull(userToUpdate);

            userToUpdate.FullName = "FN";
            userToUpdate.PhoneNumber = "123";

            _repository.UpdateProfile(userToUpdate);
            _repository.Save();

            _repository = new UserRepository(_context);
            var updatedUser = _repository.FindBy(x => x.Id == userToUpdate.Id).FirstOrDefault();
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual("FN", updatedUser.FullName);
            Assert.AreEqual("123", updatedUser.PhoneNumber);
            Console.WriteLine($"{updatedUser.FullName} {updatedUser.UserType} ,{updatedUser.FullName} {updatedUser.PhoneNumber}");
        }

        [Test]
        public void should_Get_All_Users()
        {
            var users = _repository.GetAllUsers().ToList();
            Assert.IsNotEmpty(users);
            foreach (var user in users)
            {
                Assert.IsTrue(user.UserType != UserType.Steward);
                Console.WriteLine($"{user.FullName} {user.UserType}");
            }
        }

        [Test]
        public void should_Get_AllStewards()
        {
            var stewards = _repository.GetAllStewards().ToList();
            Assert.IsNotEmpty(stewards);
            foreach (var steward in stewards)
            {
                Assert.IsTrue(steward.UserType == UserType.Steward);
                Console.WriteLine($"{steward.FullName} {steward.UserType}");
            }
        }

        [Test]
        public void should_Get_AllStewards_In_Org()
        {
            var org = _organizations.First();
            var stewards = _repository.GetAllStewardsInOrg(org.Id).ToList();
            Assert.IsNotEmpty(stewards);
            foreach (var steward in stewards)
            {
                Assert.IsTrue(steward.UserType == UserType.Steward);
                Console.WriteLine($"{steward.FullName} {steward.UserType} ,{steward.Organization.Name}");
            }
        }

        [Test]
        public void should_Get_All_Users_In_Stewards_Org()
        {
            var org = _organizations.First();
            var users = _repository.GetAllUsersInSameStewardOrg(org.Id).ToList();
            Assert.IsNotEmpty(users);
            foreach (var user in users)
            {
                Assert.IsTrue(user.UserType != UserType.Steward);
                Assert.IsTrue(user.UserType != UserType.Admin);
                Console.WriteLine($"{user.FullName} ,{user.UserType},{user.Organization.Name}");
            }
        }

        [Test]
        public void should_Get_User_Prefences()
        {
            var org = _organizations.First();
            var users = _repository.GetAllUsersInSameStewardOrg(org.Id).ToList();
            Assert.IsNotEmpty(users);
            foreach (var user in users)
            {
                Assert.IsTrue(user.UserType != UserType.Steward);
                Assert.IsTrue(user.UserType != UserType.Admin);
                Console.WriteLine($"{user.FullName} ,{user.UserType},{user.Organization.Name}");
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