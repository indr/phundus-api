namespace Phundus.Common.Projecting
{
    using System;
    using System.Linq.Expressions;
    using Castle.Core.Logging;
    using Domain.Model;
    using NHibernate;

    public abstract class ProjectionBase : IProjection
    {
        public ILogger Logger { get; set; }

        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get { return SessionFactory(); }
        }

        public abstract void Reset();

        public virtual Type GetEntityType()
        {
            return null;
        }
    }    

    public class ProjectionBase<TEntity> : ProjectionBase where TEntity : class, new()
    {
        public override Type GetEntityType()
        {
            return typeof (TEntity);
        }

        protected TEntity Get(GuidIdentity id)
        {
            if (id == null) throw new ArgumentNullException("id");

            return Session.Get<TEntity>(id.Id);
        }

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
                throw new InvalidOperationException(String.Format("Could not update projection {0}. Projection {1} {2} not found.", this.GetType().Name, typeof(TEntity).Name, id));

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

        public override void Reset()
        {
            var sql = String.Format("FROM {0}", typeof (TEntity).Name);
            Session.Delete(sql);
        }
    }
}