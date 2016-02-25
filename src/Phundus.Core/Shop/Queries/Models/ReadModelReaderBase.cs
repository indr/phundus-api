namespace Phundus.Shop.Queries.Models
{
    using Cqrs;

    public abstract class ReadModelReaderBase : ProjectionBase
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