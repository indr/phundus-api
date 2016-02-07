namespace Phundus.Cqrs
{
    public abstract class NHibernateReadModelBase<TRecord> : ReadModelBase<TRecord> where TRecord : class, new()
    {
    }
}