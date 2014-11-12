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

        public QuantityInInventoryDecreased(string stockId, int change, int total, DateTime asOfUtc)
        {
            StockId = stockId;
            Change = change;
            Total = total;
            AsOfUtc = asOfUtc;
        }

        public string StockId { get; set; }
        public int Change { get; set; }
        public int Total { get; set; }
        public DateTime AsOfUtc { get; set; }
    }
}