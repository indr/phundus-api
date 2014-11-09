namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using Catalog;
    using IdentityAndAccess.Domain.Model.Organizations;

    public interface IStockRepository
    {
        Stock Get(OrganizationId organizationId, ArticleId articleId, StockId stockId);

        StockId GetNextIdentity();

        void Save(Stock reservation);
    }
}