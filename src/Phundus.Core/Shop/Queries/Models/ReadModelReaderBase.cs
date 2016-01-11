namespace Phundus.Core.Shop.Queries.Models
{
    using Cqrs;
    using Phundus.Shop.Queries;

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