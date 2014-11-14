namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class QuantityInInventoryDecreased : DomainEvent
    {
        protected QuantityInInventoryDecreased()
        {
        }

        public QuantityInInventoryDecreased(string stockId, int change, int total, DateTime asOfUtc, string comment)
        {
            StockId = stockId;
            Change = change;
            Total = total;
            AsOfUtc = asOfUtc;
            Comment = comment;
        }

        [DataMember(Order = 1)]
        public string StockId { get; set; }

        [DataMember(Order = 2)]
        public int Change { get; set; }

        [DataMember(Order = 3)]
        public int Total { get; set; }

        [DataMember(Order = 4)]
        public DateTime AsOfUtc { get; set; }

        [DataMember(Order = 5)]
        public string Comment { get; set; }
    }
}