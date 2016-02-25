namespace Phundus.Cqrs
{
    using System;
    using System.Linq.Expressions;
    using NHibernate;

    public class ProjectionBase<TEntity> where TEntity : class, new()
    {
        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get { return SessionFactory(); }
        }

        protected void Insert(Action<TEntity> action)
        {
            var row = new TEntity();
            action(row);
            Session.Save(row);
        }

        protected void Update(object id, Action<TEntity> action)
        {
            var row = Session.Get<TEntity>(id);
            action(row);
            Session.Update(row);
        }

        protected void InsertOrUpdate(Expression<Func<TEntity, bool>> expression, Action<TEntity> action)
        {
            Session.Flush();
            var row = Single(expression) ?? new TEntity();
            action(row);
            Session.SaveOrUpdate(row);
        }

        protected void Delete(object id)
        {
            var row = Session.Get<TEntity>(id);
            Session.Delete(row);
        }

        protected TEntity Single(Expression<Func<TEntity, bool>> expression)
        {
            return Session.QueryOver<TEntity>().Where(expression).SingleOrDefault();
        }
    }
}