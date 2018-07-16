using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DWHDashboard.SharedKernel.Interfaces
{
    public interface IRepository<T> : IReadOnlyRepository<T> where T : class
    {
        void Create(T entity);

        void Create(IEnumerable<T> entities);

        void Update(T entity);

        void Update(IEnumerable<T> entities);

        void Delete(Guid id);

        void Delete(T entity);

        void Save();

        Task<int> SaveAsync();
    }
}