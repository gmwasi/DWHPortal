using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.SharedKernel.Interfaces;

namespace DWHDashboard.ProfileManagement.Core.Interfaces
{
    public interface IUserRepository:IRepository<User>
    {
        void AddOrUpdateTableaUser(User user);
        void UpdateProfile(User user);
        void DeleteUser(string id);
        void ConfirmUser(string id,UserConfirmation confirmed);
        void MakeUser(string id);
        void MakeUserSteward(string id);
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetAllStewards();
        IEnumerable<User> GetAllStewardsInOrg(Guid orgId);
        IEnumerable<User> GetAllUsersInSameStewardOrg(Guid orgId);


        Task<int> SaveUserPreference(PreferenceType preferenceType, string username, string bundle);
        IEnumerable<UserPreference> GetUserPreferences(PreferenceType preferenceType, string username);
    }
}