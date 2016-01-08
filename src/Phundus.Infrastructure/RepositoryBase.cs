namespace Phundus.Infrastructure
{
    using System;
    using Common;
    using NHibernate;

    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        private Type _concreteType;

        public Type ConcreteType
        {
            get { return _concreteType ?? typeof (T); }
            set { _concreteType = value; }
        }

        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get { return SessionFactory(); }
        }

        public T ById(object id)
        {
            return FindById(id);
        }

        public T FindById(object id)
        {
            return (T) Session.Get(ConcreteType, id);
        }

        public T GetById(object id)
        {
            var result = FindById(id);
            if (result == null)
                throw new NotFoundException();
            return result;
        }

        public void Remove(T entity)
        {
            Session.Delete(entity);
        }

        public T Add(T entity)
        {
            Session.SaveOrUpdate(entity);
            return entity;
        }

        public void Update(T entity)
        {
            Session.Update(entity);
        }
    }
}