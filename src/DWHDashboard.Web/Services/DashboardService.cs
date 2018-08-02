using DWHDashboard.ProfileManagement.Core.Interfaces;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.SharedKernel.Utility;
using DWHDashboard.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;
using DWHDashboard.SharedKernel.Model;
using Common = DWHDashboard.Web.Utils.Common;
using User = DWHDashboard.ProfileManagement.Core.Model.User;

namespace DWHDashboard.Web.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly string _baseUrl;
        private readonly ITempOrgRepository _orgRepository;
        private HttpClient _client;
        private AuthSession _authSession;
        private readonly ApplicationSettings _applicationSettings;

        public DashboardService(string baseUrl, ITempOrgRepository orgRepository, ApplicationSettings applicationSettings)
        {
            _orgRepository = orgRepository;
            _applicationSettings = applicationSettings;
            _baseUrl = Common.FixUrl(baseUrl);
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
        }

        public Task<AuthSession> Authenticate()
        {
            return Authenticate(_applicationSettings.PublicUser, _applicationSettings.PublicPassword, _applicationSettings.PublicSite);
        }

        public async Task<AuthSession> Authenticate(string username, string password, string site)
        {
            var currentsite = new Site { ContentUrl = $@"""{site}""" };

            var credentials = new Credentials
            {
                Site = currentsite,
                Name = $@"""{username}""",
                Password = $@"""{password}"""
            };

            //sign In
            var tsRequest = new SignInRequest(credentials).GetTsRequest();
            var response = await _client.PostAsync(@"auth/signin", tsRequest);
            response.EnsureSuccessStatusCode();
            var tsResponse = await response.Content.ReadAsStringAsync();

            var doc = XElement.Parse(tsResponse);

            var jsonResponse = JsonConvert.SerializeXNode(doc, Newtonsoft.Json.Formatting.Indented, true);

            var signInResponse = JsonConvert.DeserializeObject<SignInResponse>(jsonResponse);

            //get token

            _authSession = new AuthSession(signInResponse);
            _authSession.SiteName = site;
            return _authSession;
        }

        public async Task<List<Workbook>> GetAllWorkbooks()
        {
            if (null == _authSession)
            {
                await Authenticate();
            }

            //TODO: DNR
            _client = GetClient();
            _client.DefaultRequestHeaders.Add(_authSession.Header, _authSession.Token);
            var response = await _client.GetAsync($@"sites/{_authSession.SiteId}/workbooks");
            response.EnsureSuccessStatusCode();
            var tsResponse = await response.Content.ReadAsStringAsync();
            var doc = XElement.Parse(tsResponse);

            doc.Add(new XAttribute(XNamespace.Xmlns + "json", "http://james.newtonking.com/projects/json"));
            tsResponse = doc.ToString();
            tsResponse = tsResponse.Replace("<workbook id", "<workbook json:Array='true' id");

            doc = XElement.Parse(tsResponse);

            var jsonResponse = JsonConvert.SerializeXNode(doc, Newtonsoft.Json.Formatting.Indented, true);
            jsonResponse = jsonResponse.Replace("@", "");

            var workbooksResponse = JsonConvert.DeserializeObject<WorkbooksResponse>(jsonResponse);

            var workbookList = workbooksResponse.Workbooks.WorkbookList;
            foreach (var w in workbookList)
            {
                w.SiteId = _authSession.SiteId;
            }
            return workbookList;
        }

        public async Task<List<Workbook>> GetAllViewsByOrg(AuthTicket ticket, string orgId, User user)
        {
            List<string> orgViewTabids = new List<string>();

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                Guid id = new Guid(orgId);
                if (!id.IsNullOrEmpty())
                {
                    var orgViews = _orgRepository.GetOrgViews(id, true).ToList();
                    orgViewTabids = orgViews.Select(x => x.TableauId).ToList();
                }
            }

            List<Workbook> workbooksList = new List<Workbook>();

            var workbooks = await GetAllWorkbooks();

            foreach (var w in workbooks)
            {
                var views = await GetAllWorkbookViews(w.Id);

                foreach (var v in views)
                {
                    v.AuthTicket = ticket ?? new AuthTicket("-1");
                    v.InteractiveUrl = v.AuthTicket.GetViewBasePath(v.AuthSession.SiteName, v.ContentUrl.Replace("sheets/", ""));
                }
                w.AddViews(views);

                if (w.Views.Count > 0)
                    workbooksList.Add(w);
            }

            //check if admin showAll

            if (user.IsTableau || user.UserType == UserType.Admin)
                return workbooksList;

            //filter by org access

            List<Workbook> filteredWorkbooksList = new List<Workbook>();

            if (orgViewTabids.Count > 0)
            {
                foreach (var w in workbooksList)
                {
                    var views = w.Views;
                    w.Views = views.Where(x => orgViewTabids.Contains(x.Id) || (x.IsChart || x.IsPublic)).ToList();
                    if (w.Views.Count > 0)
                        filteredWorkbooksList.Add(w);
                }
            }
            else
            {
                foreach (var w in workbooksList)
                {
                    var views = w.Views;
                    w.Views = views.Where(x => x.IsChart || x.IsPublic).ToList();
                    if (w.Views.Count > 0)
                        filteredWorkbooksList.Add(w);
                }
            }
            return filteredWorkbooksList;
        }

        public async Task<List<View>> GetAllWorkbookViews(string workbookId)
        {
            if (null == _authSession)
            {
                await Authenticate();
            }

            _client = GetClient();
            _client.DefaultRequestHeaders.Add(_authSession.Header, _authSession.Token);
            var response = await _client.GetAsync($@"sites/{_authSession.SiteId}/workbooks/{workbookId}/views");
            response.EnsureSuccessStatusCode();
            var tsResponse = await response.Content.ReadAsStringAsync();

            var doc = XElement.Parse(tsResponse);

            doc.Add(new XAttribute(XNamespace.Xmlns + "json", "http://james.newtonking.com/projects/json"));
            tsResponse = doc.ToString();
            tsResponse = tsResponse.Replace("<view id", "<view json:Array='true' id");

            doc = XElement.Parse(tsResponse);

            var jsonResponse = JsonConvert.SerializeXNode(doc, Newtonsoft.Json.Formatting.Indented, true);
            jsonResponse = jsonResponse.Replace("@", "");

            var viewsResponse = JsonConvert.DeserializeObject<ViewsResponse>(jsonResponse);

            var views = viewsResponse.Views.ViewList;
            foreach (var v in views)
            {
                v.AuthSession = _authSession;
                v.WorkbookId = workbookId;
                v.PreviewImageUrl = GetPreviewImage(v);
            }
            return views;
        }

        private HttpClient GetClient()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            return _client;
        }

        public Task<List<Workbook>> GetAllWorkbooksWithViews()
        {
            return GetAllWorkbooksWithViews(null);
        }

        public async Task<List<Workbook>> GetAllWorkbooksWithViews(AuthTicket ticket)
        {
            List<Workbook> workbooksList = new List<Workbook>();

            var workbooks = await GetAllWorkbooks();
            foreach (var w in workbooks)
            {
                var views = await GetAllWorkbookViews(w.Id);
                foreach (var v in views)
                {
                    v.AuthTicket = ticket ?? new AuthTicket("-1");
                    v.InteractiveUrl = v.AuthTicket.GetViewBasePath(v.AuthSession.SiteName, v.ContentUrl.Replace("sheets/", ""));
                }
                w.AddViews(views);
                workbooksList.Add(w);
            }

            return workbooksList;
        }

        public async Task<List<Workbook>> GetAllWorkbooksWithViewsByOrg(AuthTicket ticket, string orgId, ProfileManagement.Core.Model.User user)
        {
            return await GetAllViewsByOrg(ticket, orgId, user);

            List<string> orgViewTabids = new List<string>();

            if (user.IsTableau || user.UserType == UserType.Admin)
                orgId = string.Empty;

            if (!string.IsNullOrWhiteSpace(orgId))
            {
                Guid id = new Guid(orgId);
                var orgViews = _orgRepository.GetOrgViews(id, true).ToList();
                orgViewTabids = orgViews.Select(x => x.TableauId).ToList();
            }

            List<Workbook> workbooksList = new List<Workbook>();

            var workbooks = await GetAllWorkbooks();
            foreach (var w in workbooks)
            {
                var views = await GetAllWorkbookViews(w.Id);

                if (orgViewTabids.Count > 0)
                {
                    views = views.Where(x => orgViewTabids.Contains(x.Id)).ToList();
                }

                foreach (var v in views)
                {
                    v.AuthTicket = ticket ?? new AuthTicket("-1");
                    v.InteractiveUrl = v.AuthTicket.GetViewBasePath(v.AuthSession.SiteName, v.ContentUrl.Replace("sheets/", ""));
                }
                w.AddViews(views);

                if (w.Views.Count > 0)
                    workbooksList.Add(w);
            }
            return workbooksList;
        }

        public string GetPreviewImage(View view)
        {
            return $"{_baseUrl}sites/{view.AuthSession.SiteId}/workbooks/{view.WorkbookId}/views/{view.Id}/previewImage";
        }

        public Task<AuthTicket> GetAuthTicket()
        {
            return GetAuthTicket(_applicationSettings.TicketServer, _applicationSettings.PublicUser, _applicationSettings.PublicSite);
        }

        public async Task<AuthTicket> GetAuthTicket(string ticketserver, string username, string site = "")
        {
            int lastIndex = _baseUrl.IndexOf("/api");                           //http://data.kenyahmis.org/api/2.3/
            var server = $"{_baseUrl.Substring(0, lastIndex)}/trusted";         //http://data.kenyahmis.org/trusted

            //http://data.kenyahmis.org:81/tikiti/api/Ticket

            var ticketServer = $@"{Common.FixUrl(ticketserver, false)}{(string.IsNullOrWhiteSpace(site) ? "" : $"/{site}")}";

            _client = new HttpClient();
            var response = await _client.GetAsync(ticketServer);
            response.EnsureSuccessStatusCode();

            var ticket = await response.Content.ReadAsStringAsync();

            return new AuthTicket(ticket.Replace("\"", ""), server);
        }
    }
}