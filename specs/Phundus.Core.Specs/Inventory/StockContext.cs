namespace Phundus.Core.Specs.Inventory
{
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;

    public class StockContext
    {
        public UserId InitiatorId { get; set; }
        public OrganizationId OrganizationId { get; set; }

        public ArticleId ArticleId { get; set; }
        public StockId StockId { get; set; }
    }
}