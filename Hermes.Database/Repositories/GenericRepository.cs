using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Hermes.ObjectModel;

namespace Hermes.Database.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DbContext Context;

        public GenericRepository(DbContext context)
        {
            Context = context;
        }

        public virtual IQueryable<T> All()
        {
            return Context.Set<T>();
        }

        public virtual IEnumerable<T> Where(Func<T, bool> query)
        {
            return Context.Set<T>()
                .Where(query);
        }

        public virtual T GetById(long id)
        {
            return Context.Set<T>()
                .FirstOrDefault(x => x.Id == id);
        }

        public virtual T Create(T entity)
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();

            return entity;
        }

        public virtual bool Update(long id, T entity)
        {
            Context.Set<T>().Update(entity);
            return Context.SaveChanges() > 0;
        }

        public virtual bool Update(T entity, Expression<Func<T, object>>[] properties)
        {
            var entry = Context.Set<T>().Attach(entity);

            foreach (var property in properties)
            {
                entry.Property(property).IsModified = true;
            }

            return Context.SaveChanges() > 0;
        }

        public virtual bool Delete(long id)
        {
            var entity = GetById(id);

            if (entity == null)
            {
                return false;
            }

            return Delete(entity);
        }

        public virtual bool Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
            return Context.SaveChanges() > 0;
        }
    }
}