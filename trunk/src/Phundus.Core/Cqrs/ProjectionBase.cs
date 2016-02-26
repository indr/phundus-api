namespace Phundus.Cqrs
{
    using System;
    using System.Linq.Expressions;
    using Castle.Core.Logging;
    using NHibernate;

    public class ProjectionBase
    {
        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get { return SessionFactory(); }
        }
    }

    public class ProjectionBase<TEntity> :ProjectionBase where TEntity : class, new()
    {
        public ILogger Logger { get; set; }

        protected void Insert(Action<TEntity> action)
        {
            var row = new TEntity();
            action(row);
            Session.Save(row);
        }

        protected void Insert(TEntity entity)
        {
            Session.Save(entity);
        }

        protected void Update(object id, Action<TEntity> action)
        {
            var row = Session.Get<TEntity>(id);
            if (row == null)
            {
                Logger.Warn(String.Format("Could not update projection {0}. Projection {1} not found.", typeof(TEntity).Name, id));
                return;
            }
            action(row);
            Session.Update(row);
        }

        protected void Update(Expression<Func<TEntity, bool>> expression, Action<TEntity> action)
        {
            Session.Flush();
            var entities = Session.QueryOver<TEntity>().Where(expression).Future();
            foreach (var entity in entities)
            {
                action(entity);
                Session.Update(entity);
            }
        }

        protected void InsertOrUpdate(Expression<Func<TEntity, bool>> expression, Action<TEntity> action)
        {
            Session.Flush();
            var row = SingleOrDefault(expression) ?? new TEntity();
            action(row);
            Session.SaveOrUpdate(row);
        }

        protected void InsertOrUpdate(TEntity entity)
        {
            Session.SaveOrUpdate(entity);
        }

        protected void Delete(object id)
        {
            var row = Session.Get<TEntity>(id);
            Session.Delete(row);
        }

        protected void Delete(TEntity entity)
        {
            Session.Delete(entity);
        }

        protected TEntity SingleOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            return Session.QueryOver<TEntity>().Where(expression).SingleOrDefault();
        }

        protected IQueryOver<TEntity, TEntity> QueryOver()
        {
            return Session.QueryOver<TEntity>();
        }
    }
}