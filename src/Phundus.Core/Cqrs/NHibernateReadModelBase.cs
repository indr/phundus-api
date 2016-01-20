namespace Phundus.Cqrs
{
    using NHibernate;

    public abstract class NHibernateReadModelBase<TRecord> : ReadModelBase where TRecord : class
    {
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