namespace piNuts.phundus.Infrastructure
{
    using System;
    using NHibernate;

    public class RepositoryBase<T> : IRepository<T>
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

        #region IRepository<T> Members

        public T ById(object id)
        {
            return (T) Session.Get(ConcreteType, id);
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

        #endregion
    }
}