namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System.Collections.Generic;
    using Catalog;
    using Common.Domain.Model;

    public class StockCreated : DomainEvent
    {
        public StockCreated(string stockId, int articleId)
        {
            StockId = stockId;
            ArticleId = articleId;
        }

        public string StockId { get; set; }
        public int ArticleId { get; set; }
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

        public Stock(StockId stockId, ArticleId articleId)
        {
            Apply(new StockCreated(new StockId().Id, articleId.Id));
        }

        public Stock(IEnumerable<IDomainEvent> eventStream, long streamVersion) : base(eventStream, streamVersion)
        {
        }

        public StockId StockId { get; private set; }

        public ArticleId ArticleId { get; private set; }

        public Quantity QuantityInInventory
        {
            get { return _quantityInInventory; }
            private set { _quantityInInventory = value; }
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return StockId;
        }

        protected override void When(IDomainEvent e)
        {
            When((dynamic) e);
        }

        protected void When(StockCreated e)
        {
            StockId = new StockId(e.StockId);
            ArticleId = new ArticleId(e.ArticleId);
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