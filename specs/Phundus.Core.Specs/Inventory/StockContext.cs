namespace Phundus.Core.Specs.Inventory
{
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class StockContext
    {
        public OrganizationId OrganizationId { get; set; }
        public ArticleId ArticleId { get; set; }
        public StockId StockId { get; set; }
    }
}