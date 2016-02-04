namespace Phundus.Shop.Queries.Models
{
    using System;
    using Cqrs;
    using NHibernate;

    public abstract class ReadModelReaderBase : ReadModelBase
    {
        protected ReadModelDataContext CreateCtx()
        {
            var session = Session;
            session.Flush();
            var _ctx = new ReadModelDataContext(session.Connection);
            _ctx.ObjectTrackingEnabled = true;
            return _ctx;
        }
    }
}