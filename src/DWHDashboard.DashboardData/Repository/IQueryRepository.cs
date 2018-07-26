using System.Collections.Generic;
using System.Data;

namespace DWHDashboard.DashboardData.Repository
{
    public interface IQueryRepository
    {
        dynamic Result(string query);
    }
}