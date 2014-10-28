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

        protected void Insert(TRecord record)
        {
            Session.Save(record);
        }
    }
}