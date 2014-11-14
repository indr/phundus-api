namespace Phundus.Core.Inventory.Port.Adapter.Persistence
{
    using Common.Events;
    using Domain.Model.Catalog;
    using Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class EventStoreStockRepository : EventStoreRepositoryBase, IStockRepository
    {
        public Stock Get(OrganizationId organizationId, ArticleId articleId, StockId stockId)
        {
            return Get(new EventStreamId(stockId.Id), es => new Stock(es.Events, es.Version));
        }

        public StockId GetNextIdentity()
        {
            return new StockId();
        }

        public void Save(Stock stock)
        {
            Append(stock.StockId.Id, stock);
        }
    }
}