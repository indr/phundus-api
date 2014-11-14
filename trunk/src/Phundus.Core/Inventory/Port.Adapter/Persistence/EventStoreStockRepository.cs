namespace Phundus.Core.Inventory.Port.Adapter.Persistence
{
    using System;
    using Common;
    using Common.Events;
    using Domain.Model.Catalog;
    using Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class EventStoreStockRepository : EventStoreRepositoryBase, IStockRepository
    {
        public Stock Get(OrganizationId organizationId, ArticleId articleId, StockId stockId)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(stockId, "Stock id must be provided.");

            return Get(new EventStreamId(stockId.Id), es => new Stock(es.Events, es.Version));
        }

        public StockId GetNextIdentity()
        {
            return new StockId(Guid.NewGuid().ToString());
        }

        public void Save(Stock stock)
        {
            AssertionConcern.AssertArgumentNotNull(stock, "Stock must be provided.");

            Append(stock.StockId.Id, stock);
        }
    }
}