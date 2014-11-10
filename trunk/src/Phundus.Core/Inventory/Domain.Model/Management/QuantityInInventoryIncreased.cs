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

        public QuantityInInventoryIncreased(string stockId, int quantity, int total, DateTime asOfUtc)
        {
            StockId = stockId;
            Quantity = quantity;
            Total = total;
            AsOfUtc = asOfUtc;
        }

        public string StockId { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
        public DateTime AsOfUtc { get; set; }
    }
}