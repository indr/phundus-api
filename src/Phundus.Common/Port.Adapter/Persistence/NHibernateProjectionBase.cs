namespace Phundus.Common.Port.Adapter.Persistence
{
    using System;
    using NHibernate;

    public abstract class NHibernateProjectionBase<TRecord>
    {
        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get { return SessionFactory(); }
        }

        protected TRecord Find(object id)
        {
            return Session.Get<TRecord>(id);
        }

        protected void Save(TRecord record)
        {
            Session.SaveOrUpdate(record);
        }
    }
}