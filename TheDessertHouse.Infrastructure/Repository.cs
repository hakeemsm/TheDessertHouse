using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using TheDessertHouse.Domain;

namespace TheDessertHouse.Infrastructure
{
    public class Repository : IRepository
    {
        private readonly ISession _session;

        public Repository(ISessionFactory sessionFactory)
        {
            _session = sessionFactory.OpenSession();
        }

        public void Dispose()
        {
            if(_session.IsOpen)
                _session.Close();
            
        }

        public T Get<T>(object id)
        {
            T retVal;
            using (var trx=_session.BeginTransaction())
            {
                retVal = _session.Load<T>(id);
                trx.Commit();
            }
            return retVal;
        }

        public IQueryable<T> Get<T>()
        {
            return PerformQueryTransaction(() => _session.Query<T>());
        }

        private IQueryable<T> PerformQueryTransaction<T>(Func<IQueryable<T>> func)
        {
            IQueryable<T> queryResult;
            using (var trx=_session.BeginTransaction())
            {
                queryResult = func();
                trx.Commit();
            }
            return queryResult;
        }

        public object Save(IEntity entity)
        {
            return PerformSaveUpdateTransaction(() => _session.Save(entity));
/*
            object retVal;
            using (var trx = _session.BeginTransaction())
            {
                retVal = _session.Save(entity);
                trx.Commit();
            }
            return retVal;
*/
        }

        public void Delete<T>(IEntity entity)
        {
            PerformCrudTransaction(() => _session.Delete(entity));
            
        }

        private void PerformCrudTransaction(Action crudAction)
        {
            using (var trx = _session.BeginTransaction())
            {
                crudAction();
                trx.Commit();
            }
        }

        public void SaveOrUpdate(IEntity entity)
        {
            PerformCrudTransaction(()=>_session.SaveOrUpdate(entity));
            
        }

        public void Delete<T>(object entity)
        {
            PerformCrudTransaction(() => _session.Delete(entity));
            
        }

        public void Update(IEntity entity)
        {
            PerformCrudTransaction(() => _session.Update(entity));
        }

        public object Merge(IEntity entity)
        {
            return PerformSaveUpdateTransaction(() => _session.Merge(entity));
        }

        private object PerformSaveUpdateTransaction(Func<object> func)
        {
            object retVal;
            using (var trx = _session.BeginTransaction())
            {
                retVal = func();
                trx.Commit();
            }
            return retVal;
        }
    }
}