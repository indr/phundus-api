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

        public StockCreated(int organizationId, int articleId, string stockId)
        {
            OrganizationId = organizationId;
            ArticleId = articleId;
            StockId = stockId;
        }

        [DataMember(Order = 1)]
        public int OrganizationId { get; set; }

        [DataMember(Order = 2)]
        public int ArticleId { get; set; }

        [DataMember(Order = 3)]
        public string StockId { get; set; }
    }
}