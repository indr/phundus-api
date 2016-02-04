namespace Phundus.Cqrs
{
    using System;
    using NHibernate;

    public abstract class ReadModelBase
    {
        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get
            {
                var result = SessionFactory();
                //result.FlushMode = FlushMode.Always;
                return result;
            }
        }
    }

    public abstract class ReadModelBase<TRow> : ReadModelBase where TRow : new()
    {
        protected TRow CreateRow()
        {
            var result = new TRow();
            return result;
        }

        protected void SaveOrUpdate(TRow row)
        {
            Session.SaveOrUpdate(row);
            Session.Flush();
        }

        protected void Delete(TRow row)
        {
            Session.Delete(row);
        }
    }
}