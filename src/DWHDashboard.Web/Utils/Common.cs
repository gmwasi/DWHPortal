namespace DWHDashboard.Web.Utils
{
    public class Common
    {
        public static string FixUrl(string url, bool ensureTrailing = true)
        {
            if (ensureTrailing)
            {
                return url.EndsWith("/") ? url : $"{url}/";
            }
            return url.EndsWith("/") ? url.Remove(url.Length - 1) : url;
        }
    }
}