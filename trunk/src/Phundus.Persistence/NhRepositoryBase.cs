namespace Phundus.Persistence
{
    using System;
    using System.Linq;
    using Common.Domain.Model;
    using NHibernate;
    using NHibernate.Linq;

    public abstract class NhRepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private Type _concreteType;

        private Type ConcreteType
        {
            get { return _concreteType ?? typeof (TEntity); }
            set { _concreteType = value; }
        }

        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get { return SessionFactory(); }
        }

        protected IQueryable<TEntity> Entities
        {
            get { return Session.Query<TEntity>(); }
        }

        protected IQueryOver<TEntity, TEntity> QueryOver()
        {
            return Session.QueryOver<TEntity>();
        }

        public TEntity FindById(object id)
        {
            return (TEntity) Session.Get(ConcreteType, id);
        }

        public void Remove(TEntity entity)
        {
            Session.Delete(entity);
        }

        public TEntity Add(TEntity entity)
        {
            Session.SaveOrUpdate(entity);
            return entity;
        }

        public void Update(TEntity entity)
        {
            Session.Update(entity);
        }
    }
}