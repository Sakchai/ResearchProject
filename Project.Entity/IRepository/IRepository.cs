using System;
using System.Collections.Generic;

namespace Project.Entity.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        IEnumerable<T> Get(Func<T, bool> predicate);

        T GetOne(object id);

        int Insert(T entity);

        void Delete(T entity);

        void Delete(object id);

        void Update(object id, T entity);

    }
}