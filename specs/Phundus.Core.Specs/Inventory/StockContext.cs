namespace Phundus.Core.Specs.Inventory
{
    using Contexts;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;

    public class StockContext
    {
        public StockContext()
        {
            InitiatorId = new UserId(10001);
            OrganizationId = new OrganizationId(1001);
            ArticleId = new ArticleId(100001);
            StockId = new StockId("Stock-1");
        }

        public UserId InitiatorId { get; set; }
        public OrganizationId OrganizationId { get; set; }

        public ArticleId ArticleId { get; set; }
        public StockId StockId { get; set; }
    }
}