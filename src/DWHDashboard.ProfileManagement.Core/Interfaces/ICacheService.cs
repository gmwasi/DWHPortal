using System;

namespace DWHDashboard.ProfileManagement.Core.Interfaces
{
    interface ICacheService
    {
        T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class;
    }
}