namespace Phundus.Cqrs
{
    using System;
    using Castle.Core.Logging;
    using NHibernate;

    public class QueryBase
    {
        public ILogger Logger { get; set; }

        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get { return SessionFactory(); }
        }
    }

    public class QueryBase<TEntity> : QueryBase where TEntity : class
    {
        protected IQueryOver<TEntity, TEntity> QueryOver()
        {
            return Session.QueryOver<TEntity>();
        }
    }
}