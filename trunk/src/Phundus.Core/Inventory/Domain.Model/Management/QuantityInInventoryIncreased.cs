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

        public QuantityInInventoryIncreased(string stockId, int change, int total, DateTime asOfUtc, string comment)
        {
            StockId = stockId;
            Change = change;
            Total = total;
            AsOfUtc = asOfUtc;
            Comment = comment;
        }

        public string StockId { get; set; }
        public int Change { get; set; }
        public int Total { get; set; }
        public DateTime AsOfUtc { get; set; }
        public string Comment { get; set; }
    }
}