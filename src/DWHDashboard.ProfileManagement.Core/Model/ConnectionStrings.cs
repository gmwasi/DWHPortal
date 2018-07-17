namespace DWHDashboard.ProfileManagement.Core.Model
{
    public class ConnectionStrings
    {
        public string DwapiConnection { get; set; }

        public ConnectionStrings()
        {
        }

        public ConnectionStrings(string dwapiConnection)
        {
            DwapiConnection = dwapiConnection;
        }
    }
}