using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DWHDashboard.SharedKernel.Interfaces
{
    public interface IReadOnlyRepository<T> where T : class
    {
        T Find(Guid id);

        T FindByKey(Guid id);

        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAllInclude(params Expression<Func<T, object>>[] includeProperties);
    }
}