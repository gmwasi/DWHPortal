using System.Threading.Tasks;
using DWHDashboard.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DWHDashboard.Web.Controllers
{
    [Authorize]
    public class AdhocController : Controller
    {
        private readonly IAdhocService _adhocService;

        public AdhocController(IAdhocService adhocService)
        {
            _adhocService = adhocService;
        }
        public async Task<dynamic> Get(string query)
        {
            var result = await _adhocService.GetQueryResult(query);
            return result;
        }
    }
}