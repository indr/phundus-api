namespace Phundus.Common.Querying
{
    using System;
    using System.Linq.Expressions;
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

        protected TEntity SingleOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            return QueryOver().Where(expression).SingleOrDefault();
        }
    }
}