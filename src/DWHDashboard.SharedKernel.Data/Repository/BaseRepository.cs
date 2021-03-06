﻿using DWHDashboard.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DWHDashboard.SharedKernel.Data.Repository
{
    public abstract class BaseRepository<T> : BaseReadOnlyRepository<T>, IRepository<T> where T : class
    {
        protected BaseRepository(DbContext context) : base(context)
        {
        }

        public virtual void Create(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Create(IEnumerable<T> entities)
        {
            if (null == entities)
                return;

            entities.Select(e =>
            {
                return e;
            }).ToList();

            DbSet.AddRange(entities);
        }

        public virtual void Update(T entity)
        {
            if (null == entity)
                return;

            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Update(IEnumerable<T> entities)
        {
            if (null == entities)
                return;

            foreach (var e in entities)
            {
                Update(e);
            }
        }

        public virtual void Delete(Guid id)
        {
            var entity = DbSet.Find(id);
            Delete(entity);
        }

        public virtual void Delete(T entity)
        {
            if (null != entity)
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
                Save();
            }
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }

        public virtual Task<int> SaveAsync()
        {
            return Context.SaveChangesAsync();
        }
    }
}