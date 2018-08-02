namespace DWHDashboard.Web.Models
{
    public class AuthTicket
    {
        public string Ticket { get; set; }
        public string Path { get; set; }

        public AuthTicket()
        {
        }

        public AuthTicket(string ticket, string path)
        {
            Ticket = ticket;
            Path = path;
        }

        public AuthTicket(string ticket)
        {
            Ticket = ticket;
        }

        public string GetViewBasePath(string site,string viewPath)
        {
            site = string.IsNullOrWhiteSpace(site)? "" : $"t/{site}/";


            return $@"{Path}/{Ticket}/{site}views/{viewPath}";
        }
        public override string ToString()
        {
            return $"{Ticket}";
        }
    }
}