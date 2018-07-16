using System.Collections.Generic;
using System.Threading.Tasks;
using DWHDashboard.SharedKernel.Model.Tableau;
using DWHDashboard.SharedKernel.Model.Tableau.SignIn;
using DWHDashboard.SharedKernel.Model.Tableau.Sites;
using DWHDashboard.SharedKernel.Model.Tableau.SiteUsers;

namespace DWHDashboard.ProfileManagement.Core.Interfaces
{
    public interface IAdminService
    {
        string AuthToken { get; }
        Task<SignInResponse> GetAdminAuthTokenAsync(string site="");
        Task<SitesResponse> GetAllSitesAsync();
        Task<SiteUsersResponse> GetSiteUsersAsync(string site);
        Task<User> FindUser(string username, string site);
        
    }
}