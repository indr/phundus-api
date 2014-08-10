namespace Phundus.Core.Shop.Queries.Models
{
    using Cqrs;

    public abstract class ReadModelReaderBase : ReadModelBase
    {
        private ReadModelDataContext _ctx;

        protected ReadModelDataContext Ctx
        {
            get
            {
                if (_ctx == null)
                {
                    _ctx = new ReadModelDataContext(Session.Connection);
                    _ctx.ObjectTrackingEnabled = false;
                }
                return _ctx;
            }
        }
    }
}