namespace Phundus.Shop.Queries.Models
{
    using System;
    using Cqrs;
    using NHibernate;

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