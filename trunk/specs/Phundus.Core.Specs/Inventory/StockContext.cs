namespace Phundus.Core.Specs.Inventory
{
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;

    public class StockContext
    {
        public StockId StockId { get; set; }
        public ArticleId ArticleId { get; set; }
    }
}