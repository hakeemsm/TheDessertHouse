using System;
using System.Collections.Generic;
using System.Linq;
using TheDessertHouse.Domain;

namespace TheDessertHouse.Infrastructure
{
    public interface IRepository : IDisposable
    {
        T Get<T>(object o);
        IQueryable<T> Get<T>();
        object Save(IEntity entity);
        void Delete<T>(IEntity entity);
        void SaveOrUpdate(IEntity entity);
        void Delete<T>(object entity);


        void Update(IEntity entity);
        object Merge(IEntity entity);
    }
}