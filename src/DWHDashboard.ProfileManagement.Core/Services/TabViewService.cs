using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using DWHDashboard.ProfileManagement.Core.Interfaces;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.SharedKernel.Model.Tableau.Views;
using DWHDashboard.SharedKernel.Model.Tableau.Workbooks;
using DWHDashboard.SharedKernel.Utility;
using log4net;
using Newtonsoft.Json;

namespace DWHDashboard.ProfileManagement.Core.Services
{
 public   class TabViewService: ITabViewService
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IAdminService _adminService;
        private readonly string _baseUrl;
        private readonly string _siteName;

        private readonly ITabWorkbookRepository _workbookRepository;
        private readonly ITabViewRepository _viewRepository;

        private  string _authToken;
        private  string _siteId;


        public TabViewService(IAdminService adminService, string baseUrl,string siteName, ITabWorkbookRepository workbookRepository, ITabViewRepository viewRepository)
        {
            _adminService = adminService;
            _baseUrl =Common.GetBaseUrl(baseUrl);
            _siteName = siteName;
            _workbookRepository = workbookRepository;
            _viewRepository = viewRepository;
        }

        
        public async Task<IEnumerable<TableauWorkbook>> GetAllWorkbooksAsync()
        {
            List<TableauWorkbook> tabWorkbooks;
            await GetAuthToken();

            if (string.IsNullOrWhiteSpace(_authToken))
            {
                var message = "Access is denied !";
                Log.Debug(message);
                throw new UnauthorizedAccessException(message);
            }

            if (string.IsNullOrWhiteSpace(_siteId))
            {
                var message = "Site Id required!";
                Log.Debug(message);
                throw new ArgumentException(message);
            }

            try
            {
                //GET   /api/api-version/sites/site-id/workbooks
                var response = await GetHttpClient(_authToken).GetAsync($"sites/{_siteId}/workbooks");
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                var workbooksResponse = JsonConvert.DeserializeObject<WorkbooksResponse>(content);
                tabWorkbooks = TableauWorkbook.Generate(workbooksResponse.GetWorkbooks());

                foreach (var tabWorkbook in tabWorkbooks)
                {
                    var tabViews = await GetAllViewsAsync(tabWorkbook.TableauId);

                    if (null!=tabViews)
                        tabWorkbook.AddTabViews(tabViews.ToList());
                }
            }
            catch (Exception e)
            {
                Log.Debug(e);
                throw;
            }

            return tabWorkbooks;
        }

        public async Task<IEnumerable<TableauView>> GetAllViewsAsync(string workbookId)
        {
            List<TableauView> tabViews;

            await GetAuthToken();

            if (string.IsNullOrWhiteSpace(_authToken))
            {
                var message = "Access is denied !";
                Log.Debug(message);
                throw new UnauthorizedAccessException(message);
            }

            if (string.IsNullOrWhiteSpace(_siteId))
            {
                var message = "Site Id required!";
                Log.Debug(message);
                throw new ArgumentException(message);
            }

            try
            {
                //GET   /api/api-version/sites/site-id/workbooks/workbook-id/views
                var response = await GetHttpClient(_authToken).GetAsync($"sites/{_siteId}/workbooks/{workbookId}/views");
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                var viewsResponse = JsonConvert.DeserializeObject<ViewsResponse>(content);
                tabViews = TableauView.Generate(viewsResponse.GetViews(), workbookId);
            }
            catch (Exception e)
            {
                Log.Debug(e);
                throw;
            }

            return tabViews;
        }

        public async Task<SyncDashboardSummary> SyncAsync()
        {
            SyncDashboardSummary summary = new SyncDashboardSummary();

            //sync workbooks

            var apiworkbooks = await GetAllWorkbooksAsync();
            
            if (null != apiworkbooks)
            {
                

                var workbooks = apiworkbooks.ToList();
                var views= new List<TableauView>();


                foreach (var workbook in workbooks)
                {
                    views.AddRange(workbook.TabViews);
                    workbook.TabViews = new List<TableauView>();
                }

                summary.WorkbookSummary= await _workbookRepository.AddOrUpdateAsync(workbooks);
                
                summary.ViewSummary = await _viewRepository.AddOrUpdateAsync(views);

                await _viewRepository.UpdateSections();
            }

            return summary;
        }

        public Task<IEnumerable<TableauView>> GetAllFilteredViewsAsync()
        {
            throw new NotImplementedException();
        }

        private async Task GetAuthToken()
        {
            try
            {
                var response = await _adminService.GetAdminAuthTokenAsync(_siteName);
                _authToken = response.GetAuthToken();
                _siteId = response.GetAuthSiteId();
            }
            catch (Exception e)
            {
                Log.Debug(e);
                throw;
            }
        }


        private HttpClient GetHttpClient(string authToken)
        {
            var client = new HttpClient {BaseAddress = new Uri(_baseUrl)};
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-Tableau-Auth", authToken);
            return client;
        }
    }
}
