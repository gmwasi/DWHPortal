using Dapper;
using DWHDashboard.DashboardData.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DWHDashboard.DashboardData.Repository.Implementation
{
    public class QueryRepository : IQueryRepository
    {
        private readonly DwhDataContext _dbContext;

        //todo Mwasi add error handling
        public QueryRepository(DwhDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public dynamic Result(string query)
        {
            var result = _dbContext.Database.GetDbConnection().Query(query).ToList();
            return result;
        }
    }
}