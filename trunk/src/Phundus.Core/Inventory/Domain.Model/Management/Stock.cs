namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System.Collections.Generic;
    using Catalog;
    using Common.Domain.Model;

    public class StockCreated : DomainEvent
    {
        public StockId StockId { get; set; }
        public ArticleId ArticleId { get; set; }
    }

    public class QuantityInInventoryIncreased : DomainEvent
    {
        public QuantityInInventoryIncreased(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; set; }
    }

    public class QuantityInInventoryDecreased : DomainEvent
    {
        public QuantityInInventoryDecreased(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; set; }
    }

    public class Stock : EventSourcedRootEntity
    {
        private Quantity _quantityInInventory = new Quantity(0);

        public Stock()
        {
            Apply(new StockCreated());
        }

        public Stock(IEnumerable<IDomainEvent> eventStream, long streamVersion) : base(eventStream, streamVersion)
        {
        }

        public StockId Id { get; private set; }

        public ArticleId ArticleId { get; private set; }

        public Quantity QuantityInInventory
        {
            get { return _quantityInInventory; }
            private set { _quantityInInventory = value; }
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return Id;
        }

        protected override void When(IDomainEvent e)
        {
            When((dynamic) e);
        }

        protected void When(StockCreated e)
        {
            Id = e.StockId;
            ArticleId = e.ArticleId;
        }

        public void IncreaseQuantityInInventory(int amount)
        {
            Apply(new QuantityInInventoryIncreased(amount));
        }

        protected void When(QuantityInInventoryIncreased e)
        {
            QuantityInInventory = new Quantity(e.Amount);
        }

        public void DecreaseQuantityInInventory(int amount)
        {
            Apply(new QuantityInInventoryDecreased(amount));
        }

        protected void When(QuantityInInventoryDecreased e)
        {
            QuantityInInventory = new Quantity(QuantityInInventory.Amount - e.Amount);
        }
    }
}