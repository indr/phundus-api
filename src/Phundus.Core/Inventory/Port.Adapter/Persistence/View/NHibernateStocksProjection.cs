namespace Phundus.Core.Inventory.Port.Adapter.Persistence.View
{
    using Application.Data;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Port.Adapter.Persistence;
    using Domain.Model.Management;

    public class NHibernateStocksProjection : NHibernateProjectionBase<StockData>, IDomainEventHandler
    {
        public void Handle(IDomainEvent domainEvent)
        {
            Process((dynamic) domainEvent);
        }

        private void Process(DomainEvent e)
        {
            // Fallback
        }

        private void Process(StockCreated e)
        {
            var record = Query.Where(p => p.StockId == e.StockId).SingleOrDefault();
            if (record == null)
                record = new StockData(e.StockId);

            record.ArticleId = e.ArticleId;
            Save(record);
        }
    }
}