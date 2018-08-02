using DWHDashboard.DashboardData.Models;
using DWHDashboard.DashboardData.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DWHDashboard.Web.Services
{
    public class AdhocService : IAdhocService
    {
        private readonly IDatimNewlyEnrolledRepository _datimNewlyEnrolledRepository;
        private readonly IDatimNewlyEnrolledBaselineCd4Repository _datimNewlyEnrolledBaselineCd4Repository;
        private readonly IViralLoadMatrixRepository _viralLoadMatrixRepository;
        private readonly IQueryRepository _queryRepository;

        public AdhocService(IDatimNewlyEnrolledRepository datimNewlyEnrolledRepository, IQueryRepository queryRepository, IDatimNewlyEnrolledBaselineCd4Repository datimNewlyEnrolledBaselineCd4Repository, IViralLoadMatrixRepository viralLoadMatrixRepository)
        {
            _datimNewlyEnrolledRepository = datimNewlyEnrolledRepository;
            _queryRepository = queryRepository;
            _datimNewlyEnrolledBaselineCd4Repository = datimNewlyEnrolledBaselineCd4Repository;
            _viralLoadMatrixRepository = viralLoadMatrixRepository;
        }

        public async Task<List<DatimNewlyEnrolled>> GetAllNewlyEnrolled()
        {
            return await Task.Run(() => _datimNewlyEnrolledRepository.GetAll().ToList());
        }

        public async Task<List<DatimNewlyEnrolled>> GetNewlyEnrolledBy(DateTime startDate, DateTime endDate, string county, string gender, string facility)
        {
            var result = await Task.Run(() => _datimNewlyEnrolledRepository.FindBy(n => n.StartArtDateEom >= startDate && n.StartArtDateEom <= endDate));
            if (!String.IsNullOrWhiteSpace(county))
            {
                result = result.Where(n => n.County == county);
            }
            if (!String.IsNullOrWhiteSpace(county))
            {
                result = result.Where(n => n.Gender == gender);
            }
            if (!String.IsNullOrWhiteSpace(facility))
            {
                result = result.Where(n => n.FacilityName == facility);
            }
            return result.ToList();
        }

        public async Task<List<DatimNewlyEnrolled>> GetNewlyEnrolledBy(DateTime startDate, DateTime endDate, string partner, string county, string facility, string project)
        {
            var result = await Task.Run(() => _datimNewlyEnrolledRepository.FindBy(n => n.StartArtDateEom >= startDate && n.StartArtDateEom <= endDate));
            if (!String.IsNullOrWhiteSpace(partner))
            {
                result = result.Where(n => n.ImplementingMechnanism == partner);
            }
            if (!String.IsNullOrWhiteSpace(county))
            {
                result = result.Where(n => n.County == county);
            }
            if (!String.IsNullOrWhiteSpace(facility))
            {
                result = result.Where(n => n.FacilityName == facility);
            }
            return result.ToList();
        }

        public async Task<DatimNewlyEnrolled> GetNewlyEnrolledById(int id)
        {
            return await Task.Run(() => _datimNewlyEnrolledRepository.GetById(id));
        }

        public async Task<List<DatimNewlyEnrolledBaselineCd4>> GetAllNewlyEnrolledBaseline()
        {
            return await Task.Run(() => _datimNewlyEnrolledBaselineCd4Repository.GetAll().ToList());
        }

        public async Task<List<DatimNewlyEnrolledBaselineCd4>> GetNewlyEnrolledBaselineBy(DateTime startDate, DateTime endDate, string county, string gender, string facility)
        {
            var result = await Task.Run(() => _datimNewlyEnrolledBaselineCd4Repository.FindBy(n => n.StartArtDate >= startDate && n.StartArtDate <= endDate));
            if (!String.IsNullOrWhiteSpace(county))
            {
                result = result.Where(n => n.County == county);
            }
            if (!String.IsNullOrWhiteSpace(gender))
            {
                result = result.Where(n => n.Gender == gender);
            }
            if (!String.IsNullOrWhiteSpace(facility))
            {
                result = result.Where(n => n.FacilityName == facility);
            }
            return result.ToList();
        }

        public async Task<List<DatimNewlyEnrolledBaselineCd4>> GetNewlyEnrolledBaselineBy(DateTime startDate, DateTime endDate, string partner, string county, string facility, string project)
        {
            var result = await Task.Run(() => _datimNewlyEnrolledBaselineCd4Repository.FindBy(n => n.StartArtDate >= startDate && n.StartArtDate <= endDate));
            if (!String.IsNullOrWhiteSpace(partner))
            {
                result = result.Where(n => n.ImplementingMechnanism == partner);
            }
            if (!String.IsNullOrWhiteSpace(county))
            {
                result = result.Where(n => n.County == county);
            }
            if (!String.IsNullOrWhiteSpace(facility))
            {
                result = result.Where(n => n.FacilityName == facility);
            }
            return result.ToList();
        }

        public async Task<DatimNewlyEnrolledBaselineCd4> GetNewlyEnrolledBaselineById(int id)
        {
            return await Task.Run(() => _datimNewlyEnrolledBaselineCd4Repository.GetById(id));
        }

        public async Task<List<ViralLoadMatrix>> GetAllViralLoadMatrix()
        {
            return await Task.Run(() => _viralLoadMatrixRepository.GetAll().ToList());
        }

        public async Task<List<ViralLoadMatrix>> GetViralLoadMatrixBy(DateTime startDate, DateTime endDate, string county, string gender, string facility)
        {
            var result = await Task.Run(() => _viralLoadMatrixRepository.FindBy(n => n.OrderedByDate >= startDate && n.OrderedByDate <= endDate));
            /*if (!String.IsNullOrWhiteSpace(county))
            {
                result = result.Where(n => n.County == county);
            }
            if (!String.IsNullOrWhiteSpace(gender))
            {
                result = result.Where(n => n.Gender == gender);
            }
            if (!String.IsNullOrWhiteSpace(facility))
            {
                result = result.Where(n => n.FacilityName == facility);
            }*/
            return result.ToList();
        }

        public async Task<List<ViralLoadMatrix>> GetViralLoadMatrixBy(DateTime startDate, DateTime endDate, string partner, string county, string facility, string project)
        {
            var result = await Task.Run(() => _viralLoadMatrixRepository.FindBy(n => n.OrderedByDate >= startDate && n.OrderedByDate <= endDate));
            /*if (!String.IsNullOrWhiteSpace(partner))
            {
                result = result.Where(n => n.ImplementingMechnanism == partner);
            }
            if (!String.IsNullOrWhiteSpace(county))
            {
                result = result.Where(n => n.County == county);
            }
            if (!String.IsNullOrWhiteSpace(facility))
            {
                result = result.Where(n => n.FacilityName == facility);
            }*/
            return result.ToList();
        }

        public async Task<ViralLoadMatrix> GetViralLoadMatrixById(int id)
        {
            return await Task.Run(() => _viralLoadMatrixRepository.GetById(id));
        }

        public async Task<dynamic> GetQueryResult(string query)
        {
            return await Task.Run(() => _queryRepository.Result(query));
        }
    }
}