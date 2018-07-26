using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using DWHDashboard.DashboardData.Data;
using Dapper;

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
            var result = _dbContext.Database.Connection.Query(query).ToList(); ;
            return result;
        }
    }
}