using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Hermes.ObjectModel;

namespace Hermes.Database.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> All();

        IEnumerable<T> Where(Func<T, bool> query);

        T GetById(long id);

        T Create(T entity);

        bool Update(long id, T entity);

        bool Update(T entity, params Expression<Func<T, object>>[] properties);

        bool Delete(long id);

        bool Delete(T entity);
    }
}