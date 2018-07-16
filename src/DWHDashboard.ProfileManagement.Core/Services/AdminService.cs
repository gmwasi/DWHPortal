using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DWHDashboard.ProfileManagement.Core.Interfaces;
using DWHDashboard.SharedKernel.Model.Tableau;
using DWHDashboard.SharedKernel.Model.Tableau.SignIn;
using DWHDashboard.SharedKernel.Model.Tableau.Sites;
using DWHDashboard.SharedKernel.Model.Tableau.SiteUsers;
using DWHDashboard.SharedKernel.Utility;
using Newtonsoft.Json;

namespace DWHDashboard.ProfileManagement.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly string _baseUrl;
        private readonly string _adminUser;
        private readonly string _adminPassword;
        private HttpClient _client;
        private string _authToken;
        private List<Site> _sites=new List<Site>();

        public AdminService(string baseUrl, string adminUser, string adminPassword)
        {
            _baseUrl = Common.GetBaseUrl(baseUrl);
            _adminUser = adminUser;
            _adminPassword = adminPassword;
        }

        public string AuthToken => _authToken;

        public async Task<SignInResponse> GetAdminAuthTokenAsync(string site = "")
        {
            _client = GetHttpClient();
            var signInRequest = SignInRequest.Create(_adminUser, _adminPassword, site);

            //todo migration

            /*var response = await _client.PostAsync("auth/signin", signInRequest);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();*/

            string content = "";

             var signInResponse = JsonConvert.DeserializeObject<SignInResponse>(content);

            _authToken = signInResponse.GetAuthToken();

            return signInResponse;
        }

        public async Task<SitesResponse> GetAllSitesAsync()
        {
            //get authToken
            if (string.IsNullOrWhiteSpace(AuthToken))
            {
                await GetAdminAuthTokenAsync();
            }

            _client = GetHttpClient(AuthToken);            

            var response = await _client.GetAsync("sites");
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            var sitesResponse = JsonConvert.DeserializeObject<SitesResponse>(content);

            return sitesResponse;
        }

        public async Task<SiteUsersResponse> GetSiteUsersAsync(string site)
        {
            //get authToken
            await GetAdminAuthTokenAsync(site);

            var siteRes= await GetAllSitesAsync();
            if (null != siteRes)
            {
                _sites = siteRes.GetSites();
            }

            var userSite = FindSite(site);

            if(null==userSite)
                throw new ArgumentException($"Site {site} NOT Found !");
            
            _client = GetHttpClient(AuthToken);

            var response = await _client.GetAsync($"sites/{userSite.Id}/users?fields=_all_&pageSize=1000");
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            var siteUsersResponse = JsonConvert.DeserializeObject<SiteUsersResponse>(content);

            return siteUsersResponse;
        }
      
        public async Task<User> FindUser(string username, string site)
        {
            User user = null;
            var siteUsersResponse = await GetSiteUsersAsync(site);

            if (null != siteUsersResponse)
            {
                var users = siteUsersResponse.GetUsers();

               user = users
                    .FirstOrDefault(x => x.Name.ToLower() == username.ToLower());
            }
            return user;
        }
        
        private Site FindSite(string site)
        {
            return _sites
                .FirstOrDefault(x => x.ContentUrl.ToLower() == site.ToLower() ||
                                     x.Name.ToLower() == site.ToLower());
        }

        private HttpClient GetHttpClient(string authToken="")
        {
            _client = new HttpClient {BaseAddress = new Uri(_baseUrl)};
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrWhiteSpace(authToken))
            {
                _client.DefaultRequestHeaders.Add("X-Tableau-Auth", authToken);                
            }
            return _client;
        }
        
    }
}
