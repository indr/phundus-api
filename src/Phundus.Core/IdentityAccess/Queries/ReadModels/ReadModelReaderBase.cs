namespace Phundus.Core.IdentityAndAccess.Queries
{
    using Cqrs;
    using IdentityAccess.Queries;

    public class ReadModelReaderBase : ReadModelBase
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