using DWHDashboard.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DWHDashboard.Web.Services
{
    public interface IDashboardService
    {
        Task<AuthSession> Authenticate();

        Task<AuthSession> Authenticate(string username, string password, string site);

        Task<AuthTicket> GetAuthTicket();

        Task<AuthTicket> GetAuthTicket(string ticketserver, string username, string site = "");

        Task<List<Workbook>> GetAllWorkbooks();

        Task<List<Workbook>> GetAllWorkbooksWithViews();

        /// <summary>
        /// Get Tableau workbooks
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        Task<List<Workbook>> GetAllWorkbooksWithViews(AuthTicket ticket);

        Task<List<Workbook>> GetAllWorkbooksWithViewsByOrg(AuthTicket ticket, string orgId, ProfileManagement.Core.Model.User user);

        Task<List<Workbook>> GetAllViewsByOrg(AuthTicket ticket, string orgId, ProfileManagement.Core.Model.User user);

        /// <summary>
        /// Get Tableau workbook.views
        /// </summary>
        /// <param name="workbookId"></param>
        /// <returns></returns>
        Task<List<View>> GetAllWorkbookViews(string workbookId);

        string GetPreviewImage(View view);
    }
}