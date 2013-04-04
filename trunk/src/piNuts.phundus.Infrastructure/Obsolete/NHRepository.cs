namespace piNuts.phundus.Infrastructure.Obsolete
{
    using System;
    using NHibernate;
    using NHibernate.Criterion;
    using piNuts.phundus.Infrastructure;

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
                return GlobalContainer.Resolve<Func<ISession>>()();
            }
        }
    }
}