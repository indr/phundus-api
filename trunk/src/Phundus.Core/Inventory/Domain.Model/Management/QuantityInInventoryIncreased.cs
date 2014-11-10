namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class QuantityInInventoryIncreased : DomainEvent
    {
        protected QuantityInInventoryIncreased()
        {
        }

        public QuantityInInventoryIncreased(string stockId, int quantity)
        {
            StockId = stockId;
            Quantity = quantity;
        }

        public string StockId { get; set; }
        public int Quantity { get; set; }
    }
}