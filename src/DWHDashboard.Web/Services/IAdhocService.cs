using DWHDashboard.DashboardData.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DWHDashboard.Web.Services
{
    public interface IAdhocService
    {
        Task<List<DatimNewlyEnrolled>> GetAllNewlyEnrolled();

        Task<DatimNewlyEnrolled> GetNewlyEnrolledById(int id);

        Task<List<DatimNewlyEnrolled>> GetNewlyEnrolledBy(DateTime startDate, DateTime endDate, string county, string gender, string facility);

        Task<List<DatimNewlyEnrolled>> GetNewlyEnrolledBy(DateTime startDate, DateTime endDate, string partner, string county, string facility, string project);

        Task<dynamic> GetQueryResult(string query);

        Task<List<DatimNewlyEnrolledBaselineCd4>> GetAllNewlyEnrolledBaseline();

        Task<List<DatimNewlyEnrolledBaselineCd4>> GetNewlyEnrolledBaselineBy(DateTime startDate, DateTime endDate, string county, string gender, string facility);

        Task<List<DatimNewlyEnrolledBaselineCd4>> GetNewlyEnrolledBaselineBy(DateTime startDate, DateTime endDate, string partner, string county, string facility, string project);

        Task<DatimNewlyEnrolledBaselineCd4> GetNewlyEnrolledBaselineById(int id);

        Task<List<ViralLoadMatrix>> GetAllViralLoadMatrix();

        Task<List<ViralLoadMatrix>> GetViralLoadMatrixBy(DateTime startDate, DateTime endDate, string county, string gender, string facility);

        Task<List<ViralLoadMatrix>> GetViralLoadMatrixBy(DateTime startDate, DateTime endDate, string partner, string county, string facility, string project);

        Task<ViralLoadMatrix> GetViralLoadMatrixById(int id);
    }
}