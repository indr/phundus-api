namespace piNuts.phundus.Infrastructure
{
    using System;
    using NHibernate;
    using piNuts.phundus.Infrastructure.Obsolete;

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

        public T Get(object id)
        {
            return (T) Session.Get(ConcreteType, id);
        }

        public void Delete(T entity)
        {
            Session.Delete((object) entity);
        }

        public T Save(T entity)
        {
            Session.Save((object) entity);
            return entity;
        }

        public T SaveOrUpdate(T entity)
        {
            Session.SaveOrUpdate((object) entity);
            return entity;
        }

        public void Update(T entity)
        {
            Session.Update((object) entity);
        }

        #endregion
    }
}