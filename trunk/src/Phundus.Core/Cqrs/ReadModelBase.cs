namespace Phundus.Core.Cqrs
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
    }

    public abstract class NHibernateReadModelBase<TRecord> : ReadModelBase where TRecord : class
    {
        protected IQueryOver<TRecord, TRecord> Query()
        {
            return Session.QueryOver<TRecord>();
        }

        protected void Insert(TRecord record)
        {
            Session.Save(record);
        }
    }
}