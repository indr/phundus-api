﻿namespace Phundus.Core.IdentityAndAccess.Queries
{
    using Cqrs;

    public class ReadModelWriterBase : ReadModelBase
    {
        private ReadModelDataContext _ctx;

        protected ReadModelDataContext Ctx
        {
            get
            {
                if (_ctx == null)
                    _ctx = new ReadModelDataContext(Session.Connection);
                return _ctx;
            }
        }
    }
}