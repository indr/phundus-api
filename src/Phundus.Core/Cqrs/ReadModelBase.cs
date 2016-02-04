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
    }

    public abstract class ReadModelBase<TRow> : ReadModelBase where TRow : new()
    {
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