using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DWHDashboard.DashboardData.Models;
using DWHDashboard.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DWHDashboard.Web.Controllers
{
    [Authorize]
    public class NewlyEnrolledController : Controller
    {
        private readonly IAdhocService _adhocService;

        public NewlyEnrolledController(IAdhocService adhocService)
        {
            _adhocService = adhocService;
        }

        // GET api/Adhoc/Get
        public async Task<List<DatimNewlyEnrolled>> Get()
        {
            var newEnrolled = await _adhocService.GetAllNewlyEnrolled();
            return newEnrolled;
        }

        // GET api/Adhoc/Get/5
        public async Task<DatimNewlyEnrolled> Get(int id)
        {
            var newEnrolled = await _adhocService.GetNewlyEnrolledById(id);
            return newEnrolled;
        }

        //// GET api/Adhoc/Get/...
        public async Task<List<DatimNewlyEnrolled>> Get(DateTime startDate, DateTime endDate, string county, string gender, string facility)
        {
            var newEnrolled = await _adhocService.GetNewlyEnrolledBy(startDate, endDate, county, gender, facility);
            return newEnrolled;
        }

        //// GET api/Adhoc/Get/...
        public async Task<List<DatimNewlyEnrolled>> Get(DateTime startDate, DateTime endDate, string partner, string county, string facility, string project)
        {
            var newEnrolled = await _adhocService.GetNewlyEnrolledBy(startDate, endDate, partner, county, facility, project);
            return newEnrolled;
        }
    }
}