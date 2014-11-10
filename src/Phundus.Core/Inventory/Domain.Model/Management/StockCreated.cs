namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class StockCreated : DomainEvent
    {
        protected StockCreated()
        {
        }

        public StockCreated(string stockId, int articleId)
        {
            StockId = stockId;
            ArticleId = articleId;
        }

        public string StockId { get; set; }
        public int ArticleId { get; set; }
    }
}