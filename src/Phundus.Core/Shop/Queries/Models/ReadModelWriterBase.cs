﻿namespace Phundus.Core.Shop.Queries.Models
{
    using Cqrs;
    using Phundus.Shop.Queries;

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