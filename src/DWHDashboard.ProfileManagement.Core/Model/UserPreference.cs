using DWHDashboard.SharedKernel;

namespace DWHDashboard.ProfileManagement.Core.Model
{
    public class UserPreference:Entity
    {
        public string UserName { get; set; }
        PreferenceType PreferenceType { get; set; }
        public string Bundle { get; set; }

        private UserPreference(string userName, PreferenceType preferenceType, string bundle)
        {
            UserName = userName;
            PreferenceType = preferenceType;
            Bundle = bundle;
        }

        public static UserPreference Create(string userName, PreferenceType preferenceType, string bundle)
        {
            return new UserPreference(userName, preferenceType, bundle);
        }
    }

    public enum PreferenceType
    {
        Filters,
        Charts
    }
}