namespace Phundus.Cqrs
{
    using System;
    using NHibernate;

    public abstract class NHibernateReadModelBase<TRecord> : ReadModelBase<TRecord> where TRecord : class, new()
    {
        protected NHibernateReadModelBase(Func<ISession> sessionFactory) : base(sessionFactory)
        {
        }

        protected IQueryOver<TRecord, TRecord> QueryOver()
        {
            return Session.QueryOver<TRecord>();
        }

        protected void Insert(TRecord record)
        {
            Session.Save(record);
        }
    }
}