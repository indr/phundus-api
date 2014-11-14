namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class QuantityInInventoryIncreased : DomainEvent
    {
        protected QuantityInInventoryIncreased()
        {
        }

        public QuantityInInventoryIncreased(int organizationId, int articleId, string stockId, int change, int total, DateTime asOfUtc, string comment)
        {
            OrganizationId = organizationId;
            ArticleId = articleId;
            StockId = stockId;
            Change = change;
            Total = total;
            AsOfUtc = asOfUtc;
            Comment = comment;
        }

        [DataMember(Order = 1)]
        public int OrganizationId { get; set; }

        [DataMember(Order = 2)]
        public int ArticleId { get; set; }

        [DataMember(Order = 3)]
        public string StockId { get; set; }

        [DataMember(Order = 4)]
        public int Change { get; set; }

        [DataMember(Order = 5)]
        public int Total { get; set; }

        [DataMember(Order = 6)]
        public DateTime AsOfUtc { get; set; }

        [DataMember(Order = 7)]
        public string Comment { get; set; }
    }
}