using DWHDashboard.SharedKernel.Interfaces;

namespace DWHDashboard.SharedKernel.Model
{
    public class ApplicationSettings
    {
        public ApplicationSettings(string server, string publicUser, string publicPassword, string serverApi, string serverJs, string ticketServer, string publicSite, string apiAdminUser, string apiAdminPassword)
        {
            Server = server;
            PublicUser = publicUser;
            PublicPassword = publicPassword;
            ServerApi = serverApi;
            ServerJs = serverJs;
            TicketServer = ticketServer;
            PublicSite = publicSite;
            ApiAdminUser = apiAdminUser;
            ApiAdminPassword = apiAdminPassword;
        }

        public string Server { get; set; }

        public string PublicUser { get; set; }

        public string PublicPassword { get; set; }

        public string ServerApi { get; set; }

        public string ServerJs { get; set; }

        public string TicketServer { get; set; }

        public string PublicSite { get; set; }

        public string ApiAdminUser { get; set; }

        public string ApiAdminPassword { get; set; }
    }
}