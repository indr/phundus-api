namespace Phundus.Cqrs
{
    using System;
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

        protected void Delete(TRow row)
        {
            Session.Delete(row);
        }

        protected void Insert(TRow row)
        {
            Session.Save(row);
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
}