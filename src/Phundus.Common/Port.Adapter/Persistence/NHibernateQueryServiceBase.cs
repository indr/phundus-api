namespace Phundus.Common.Port.Adapter.Persistence
{
    using System;
    using NHibernate;

    public abstract class NHibernateQueryServiceBase<TRecord> where TRecord : class
    {
        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get { return SessionFactory(); }
        }

        protected IQueryOver<TRecord, TRecord> Query
        {
            get { return Session.QueryOver<TRecord>(); }
        }
    }
}