using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DWHDashboard.DashboardData.Repository
{
    public interface IRepository<T>
    {
        void Insert(T entity);

        void Delete(T entity);

        IQueryable<T> GetBy(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll();

        T GetById(int id);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
    }
}