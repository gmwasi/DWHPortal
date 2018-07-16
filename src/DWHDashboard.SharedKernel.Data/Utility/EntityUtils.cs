using System;
using System.Linq.Expressions;

namespace DWHDashboard.SharedKernel.Data.Utility
{
    public static class EntityUtils
    {
        public static Expression<Func<TEntity, bool>> BuildLambdaForFindByKey<TEntity>(Guid id)
        {
            var item = Expression.Parameter(typeof(TEntity), "entity");            
            var prop = Expression.Property(item, "Id"); // or Expression.Property(item, typeof(TEntity).Name + "Id");
            var value = Expression.Constant(id);
            var equal = Expression.Equal(prop, value);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, item);
            return lambda;
        }
    }
}