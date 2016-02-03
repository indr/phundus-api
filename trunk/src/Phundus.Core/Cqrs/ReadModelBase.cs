namespace Phundus.Cqrs
{
    using System;
    using NHibernate;

    public abstract class ReadModelBase
    {
        private readonly Func<ISession> _sessionFactory;

        public ReadModelBase(Func<ISession> sessionFactory)
        {
            if (sessionFactory == null) throw new ArgumentNullException("sessionFactory");
            _sessionFactory = sessionFactory;
        }

        protected virtual ISession Session
        {
            get { return _sessionFactory(); }
        }
    }

    public abstract class ReadModelBase<TRow> : ReadModelBase where TRow : new()
    {
        protected ReadModelBase(Func<ISession> sessionFactory) : base(sessionFactory)
        {
        }

        protected TRow CreateRow()
        {
            var result = new TRow();
            Session.Save(result);
            return result;
        }

        protected void Delete(TRow row)
        {
            Session.Delete(row);
        }
    }
}