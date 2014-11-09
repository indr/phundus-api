namespace Phundus.Core.Inventory.Application.Commands
{
    public class DecreaseQuantityInInventory
    {
        public DecreaseQuantityInInventory(int initiatorId, int organizationId, int articleId, string stockId, int amount)
        {
            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            ArticleId = articleId;
            StockId = stockId;
            Amount = amount;
        }

        public int InitiatorId { get; private set; }
        public int OrganizationId { get; private set; }
        public int ArticleId { get; private set; }
        public string StockId { get; private set; }
        public int Amount { get; private set; }
    }
}