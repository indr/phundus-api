namespace Phundus.Cqrs
{
    using System;
    using System.Linq.Expressions;
    using Common;
    using NHibernate;

    public abstract class ReadModelBase
    {
        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get { return SessionFactory(); }
        }

        protected virtual IQueryOver<T, T> QueryOver<T>() where T : class
        {
            return Session.QueryOver<T>();
        }
    }

    public abstract class ReadModelBase<TRow> : ReadModelBase where TRow : class, new()
    {
        protected TRow CreateRow()
        {            
            var result = new TRow();
            return result;
        }

        protected TRow Get(object id)
        {
            return Session.Get<TRow>(id);
        }

        protected void Insert(TRow row)
        {
            Session.Save(row);
            Session.Flush();
        }

        protected void Delete(TRow row)
        {
            Session.Delete(row);
            Session.Flush();
        }

        protected void SaveOrUpdate(TRow row)
        {
            Session.SaveOrUpdate(row);
            Session.Flush();
        }

        protected IQueryOver<TRow, TRow> QueryOver()
        {
            return Session.QueryOver<TRow>();
        }
    }

    public class ProjectionBase<TRow> : ReadModelBase<TRow> where TRow : class, new()
    {
        protected void Insert(Action<TRow> action)
        {
            var row = CreateRow();
            action(row);
            Insert(row);
        }

        protected void Update(object id, Action<TRow> action)
        {
            var row = Get(id);
            action(row);
            SaveOrUpdate(row);
        }

        protected void Delete(object id)
        {
            var row = Get(id);
            base.Delete(row);
        }

        protected TRow Single(Expression<Func<TRow, bool>> expression)
        {
            return Session.QueryOver<TRow>().Where(expression).SingleOrDefault();
        }
    }
}