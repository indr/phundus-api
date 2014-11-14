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

        [DataMember(Order = 1)]
        public string StockId { get; set; }

        [DataMember(Order = 2)]
        public int ArticleId { get; set; }
    }
}