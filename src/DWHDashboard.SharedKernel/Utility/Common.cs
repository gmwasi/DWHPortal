namespace DWHDashboard.SharedKernel.Utility
{
    public class Common
    {
        public static string GetBaseUrl(string baseUrl)
        {
            return baseUrl.EndsWith(@"/") ? baseUrl : $"{baseUrl}/";
        }

        public static bool PasswordsMatch(string passwordA, string passwordB)
        {
            return string.CompareOrdinal(passwordA, passwordB) == 0;
        }
    }
}