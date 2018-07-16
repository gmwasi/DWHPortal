using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DWHDashboard.ProfileManagement.Core.Interfaces;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.ProfileManagement.Infrastructure.Data;
using DWHDashboard.SharedKernel.Data.Repository;

namespace DWHDashboard.ProfileManagement.Infrastructure.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DwhDashboardContext context) : base(context)
        {
        }

        public void AddOrUpdateTableaUser(User user)
        {
            //Todo Brian Verify
            var foundUser = FindByKey(new Guid(user.Id));

            if (null != foundUser)
            {
                Create(user);
            }
            else
            {
                Update(user);
            }
        }

        public void UpdateProfile(User user)
        {
            var userprofile =GetAll().FirstOrDefault(x=>x.Id.ToLower()==user.Id.ToLower());

            if(null==userprofile)
                throw new ArgumentException($"User [{user.FullName}] not found in System");

            userprofile.ChangeProfileTo(user);

            Update(userprofile);
        }

        public void DeleteUser(string id)
        {
            var userToDelete = GetAll().FirstOrDefault(x => x.Id.ToLower() ==id.ToLower());

            if (null == userToDelete)
                throw new ArgumentException($"User not found in System");
            Delete(userToDelete);
        }

        public void CondirmUser(string id, UserConfirmation confirmed)
        {
            var userprofile = GetAll().FirstOrDefault(x => x.Id.ToLower() == id.ToLower());

            if (null == userprofile)
                throw new ArgumentException($"User not found in System");

            userprofile.UserConfirmed = confirmed;
            userprofile.UserType = confirmed == UserConfirmation.Confirmed ? UserType.Normal : UserType.Guest;

            Update(userprofile);
        }

        public void MakeUserSteward(string id)
        {
            var userprofile = GetAll().FirstOrDefault(x => x.Id.ToLower() == id.ToLower());

            if (null == userprofile)
                throw new ArgumentException($"User not found in System");

            userprofile.UserConfirmed = UserConfirmation.Confirmed;
            userprofile.UserType = UserType.Steward;

            Update(userprofile);
        }
        public void MakeUser(string id)
        {
            var userprofile = GetAll().FirstOrDefault(x => x.Id.ToLower() == id.ToLower());

            if (null == userprofile)
                throw new ArgumentException($"User not found in System");

            userprofile.UserConfirmed = UserConfirmation.Confirmed;
            userprofile.UserType = UserType.Normal;

            Update(userprofile);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return GetAll().Where(x => x.UserType != UserType.Steward).ToList();
        }

        public IEnumerable<User> GetAllStewards()
        {
            return GetAll().Where(x => x.UserType == UserType.Steward).ToList();
        }

        public IEnumerable<User> GetAllStewardsInOrg(Guid orgId)
        {
            return GetAll()
                .Where(x => x.UserType == UserType.Steward &&
                            x.OrganizationId == orgId)
                .ToList();
        }
     
        public IEnumerable<User> GetAllUsersInSameStewardOrg(Guid orgId)
        {
            return GetAll()
                .Where(x => x.UserType != UserType.Steward &&
                            x.UserType != UserType.Admin &&
                            x.OrganizationId == orgId)
                .ToList();
        }

        public async Task<int> SaveUserPreference(PreferenceType preferenceType, string username, string bundle)
        {
            var context = Context as DwhDashboardContext;

            var preferences = context.UserPreferences.Where(x => x.UserName.ToLower() == username.ToLower()).ToList();
            context.UserPreferences.RemoveRange(preferences);
            await context.SaveChangesAsync();

            var userPreference = UserPreference.Create(username, preferenceType, bundle);
            context.UserPreferences.Attach(userPreference);
            return await context.SaveChangesAsync();
        }

        public IEnumerable<UserPreference> GetUserPreferences(PreferenceType preferenceType, string username)
        {
            return ((DwhDashboardContext) Context).UserPreferences.Where(
                x => x.UserName.ToLower() == username.ToLower()).ToList();
        }
    }
}