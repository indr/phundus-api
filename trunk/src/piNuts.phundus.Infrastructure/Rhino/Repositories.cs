// ReSharper disable CheckNamespace
namespace Rhino.Commons
// ReSharper restore CheckNamespace
{
    using System;
    using NHibernate;
    using NHibernate.Criterion;

    public class NHRepository<T> : IRepository<T>
    {
        public T Get(object id)
        {
            throw new InvalidOperationException();    
        }

        public T Load(object id)
        {
            throw new InvalidOperationException();
        }

        public void DeleteAll(DetachedCriteria @where)
        {
            throw new InvalidOperationException();
        }

        public T Save(T entity)
        {
            throw new InvalidOperationException();
        }

        public T SaveOrUpdate(T entity)
        {
            throw new InvalidOperationException();
        }

        public T SaveOrUpdateCopy(T entity)
        {
            throw new InvalidOperationException();
        }

        public void Update(T entity)
        {
            throw new InvalidOperationException();
        }

        public T FindFirst(params Order[] orders)
        {
            throw new InvalidOperationException();
        }

        public void Delete(T entity)
        {
            throw new InvalidOperationException();
        }

        public void DeleteAll()
        {
            throw new InvalidOperationException();
        }

        public Type ConcreteType
        {
            get
            {
                throw new InvalidOperationException();    
            }
            set
            {
                throw new InvalidOperationException();    
            }
        }

        protected virtual ISession Session
        {
            get
            {
                throw new InvalidOperationException();    
            }
        }
    }

    public interface IRepository<T>
    {
        T Get(object id);
        T Load(object id);
        void Delete(T entity);
        void DeleteAll();
        void DeleteAll(DetachedCriteria where);
        T Save(T entity);
        T SaveOrUpdate(T entity);
        T SaveOrUpdateCopy(T entity);
        void Update(T entity);
        T FindFirst(params Order[] orders);
    }
}