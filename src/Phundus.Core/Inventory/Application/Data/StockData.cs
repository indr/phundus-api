namespace Phundus.Core.Inventory.Application.Data
{
    public class StockData
    {
        public StockData(string stockId)
        {
            StockId = stockId;
        }

        public int ArticleId { get; set; }
        public string StockId { get; set; }
    }
}