namespace Phundus.Common.Port.Adapter.Persistence
{
    using System;
    using NHibernate;

    public abstract class NHibernateProjectionBase
    {
        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get { return SessionFactory(); }
        }

        protected TRecord Find<TRecord>(object id)
        {
            return Session.Get<TRecord>(id);
        }

        protected void Save(object record)
        {
            Session.SaveOrUpdate(record);
        }

        protected void Delete(object record)
        {
            Session.Delete(record);
        }

        protected IQueryOver<TRecord, TRecord> QueryOver<TRecord>() where TRecord : class
        {
            return Session.QueryOver<TRecord>();
        }
    }

    public abstract class NHibernateProjectionBase<TRecord> : NHibernateProjectionBase where TRecord : class
    {
        protected TRecord Find(object id)
        {
            return base.Find<TRecord>(id);
        }

        protected IQueryOver<TRecord, TRecord> Query
        {
            get { return base.QueryOver<TRecord>(); }
        }
    }
}