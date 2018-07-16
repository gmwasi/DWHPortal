using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DWHDashboard.SharedKernel.Data.Utility;
using DWHDashboard.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DWHDashboard.SharedKernel.Data.Repository
{

    public abstract class BaseReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
    {
        protected internal DbContext Context;
        protected internal DbSet<T> DbSet;

        protected BaseReadOnlyRepository(DbContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public T Find(Guid id)
        {
            return DbSet.Find(id);
        }

        public virtual T FindByKey(Guid id)
        {
            Expression<Func<T, bool>> lambda = EntityUtils.BuildLambdaForFindByKey<T>(id);
            return DbSet.SingleOrDefault(lambda);
        }

        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            var results = DbSet.AsNoTracking();

            return results.Where(predicate);
        }

        public virtual IEnumerable<T> GetAll()
        {
            var results = DbSet;
            return results;
        }

        public virtual IEnumerable<T> GetAllInclude(params Expression<Func<T, object>>[] includeProperties)
        {
            return GetAllIncluding(includeProperties).ToList();
        }

        private  IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = DbSet;

            return includeProperties.Aggregate
                (queryable, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}

