namespace Phundus.Common.Querying
{
    using System;
    using System.Linq.Expressions;
    using Castle.Core.Logging;
    using Domain.Model;
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
        protected TEntity Find(object id)
        {
            if (id == null) throw new ArgumentNullException("id");

            return Session.Get<TEntity>(id);
        }

        protected TEntity Find(GuidIdentity identity)
        {
            return Find(identity.Id);
        }

        protected TEntity SingleOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            return QueryOver().Where(expression).SingleOrDefault();
        }

        protected TEntity SingleOrThrow(Expression<Func<TEntity, bool>> expression, string format, object arg0)
        {
            var result = SingleOrDefault(expression);
            if (result == default(TEntity))
                throw new NotFoundException(format, arg0);
            return result;
        }

        protected IQueryOver<TEntity, TEntity> QueryOver()
        {
            return Session.QueryOver<TEntity>();
        }
    }
}