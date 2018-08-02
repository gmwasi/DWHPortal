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
    public class ViralLoadMatrixController : Controller
    {
        private readonly IAdhocService _adhocService;

        public ViralLoadMatrixController(IAdhocService adhocService)
        {
            _adhocService = adhocService;
        }

        // GET api/Adhoc/Get
        public async Task<List<ViralLoadMatrix>> Get()
        {
            var newEnrolled = await _adhocService.GetAllViralLoadMatrix();
            return newEnrolled;
        }

        // GET api/Adhoc/Get/5
        public async Task<ViralLoadMatrix> Get(int id)
        {
            var newEnrolled = await _adhocService.GetViralLoadMatrixById(id);
            return newEnrolled;
        }

        //// GET api/Adhoc/Get/...
        public async Task<List<ViralLoadMatrix>> Get(DateTime startDate, DateTime endDate, string county, string gender, string facility)
        {
            var newEnrolled = await _adhocService.GetViralLoadMatrixBy(startDate, endDate, county, gender, facility);
            return newEnrolled;
        }

        //// GET api/Adhoc/Get/...
        public async Task<List<ViralLoadMatrix>> Get(DateTime startDate, DateTime endDate, string partner, string county, string facility, string project)
        {
            var newEnrolled = await _adhocService.GetViralLoadMatrixBy(startDate, endDate, partner, county, facility, project);
            return newEnrolled;
        }
    }
}