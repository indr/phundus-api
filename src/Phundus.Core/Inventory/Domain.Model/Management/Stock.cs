namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System.Collections.Generic;
    using Catalog;
    using Common.Domain.Model;

    public class Stock : EventSourcedRootEntity
    {
        private Quantity _quantityInInventory = new Quantity(0);

        public Stock(StockId stockId, ArticleId articleId)
        {
            // TODO: A unit test should fail here
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

        public void IncreaseQuantityInInventory(int quantity)
        {
            Apply(new QuantityInInventoryIncreased(StockId.Id, quantity));
        }

        protected void When(QuantityInInventoryIncreased e)
        {
            // TODO: Unit test should fail here: Add the quantity
            QuantityInInventory = new Quantity(e.Quantity);
        }

        public void DecreaseQuantityInInventory(int quantity)
        {
            Apply(new QuantityInInventoryDecreased(StockId.Id, quantity));
        }

        protected void When(QuantityInInventoryDecreased e)
        {
            // TODO: Unit test should fail here: Substract the quantity
            QuantityInInventory = new Quantity(e.Quantity);
        }
    }
}