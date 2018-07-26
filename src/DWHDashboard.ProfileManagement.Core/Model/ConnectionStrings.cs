namespace DWHDashboard.ProfileManagement.Core.Model
{
    public class ConnectionStrings
    {
        public string DwhDashboardConnection { get; set; }
        public string DwhDataConnection { get; set; }

        public ConnectionStrings()
        {
        }

        public ConnectionStrings(string dwhDashboardConnection, string dwhDataConnection)
        {
            DwhDashboardConnection = dwhDashboardConnection;
            DwhDataConnection = dwhDataConnection;
        }
    }
}