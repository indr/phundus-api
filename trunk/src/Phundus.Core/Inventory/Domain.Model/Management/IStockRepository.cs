namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using IdentityAndAccess.Domain.Model.Organizations;

    public interface IStockRepository
    {
        Stock Get(OrganizationId organizationId, StockId stockId);

        StockId GetNextIdentity();

        void Save(Stock stock);
    }
}