namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using System;
    using Contexts;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;
    using NUnit.Framework;
    using Rhino.Mocks;
    using TechTalk.SpecFlow;

    [Binding]
    public class QuantityInInventorySteps
    {
        private readonly Container _container;
        private readonly PastEvents _pastEvents;
        private readonly MutatingEvents _mutatingEvents;

        private ArticleId _articleId;
        private StockId _stockId;
        

        public QuantityInInventorySteps(Container container, PastEvents pastEvents, MutatingEvents mutatingEvents)
        {
            _container = container;
            _pastEvents = pastEvents;
            _mutatingEvents = mutatingEvents;

            var repository = _container.Depend.On<IStockRepository>();

            repository.Expect(
                x => x.Get(Arg<OrganizationId>.Is.Anything, Arg<ArticleId>.Is.Anything, Arg<StockId>.Is.Anything))
                .WhenCalled(a => a.ReturnValue = new Stock(pastEvents.Events, 1)).Return(null);

            repository.Expect(x => x.Save(Arg<Stock>.Is.NotNull))
                .WhenCalled(a => _mutatingEvents.Events = ((Stock) a.Arguments[0]).MutatingEvents);
        }

        [Given(@"stock created (.*)")]
        public void StockCreated(string stockId)
        {
            _stockId = new StockId(stockId);
            _articleId = new ArticleId(1);
            _pastEvents.Add(new StockCreated(_stockId.Id, _articleId.Id));
        }

        [Given(@"quantity in inventory increased of (.*) to (.*) as of (.*)")]
        public void GivenQuantityInInventoryIncreased(int quantity, int total, DateTime asOfUtc)
        {
            _pastEvents.Add(new QuantityInInventoryIncreased(_stockId.Id, quantity, total, asOfUtc));
        }

        [When(@"Increase quantity in inventory of (.*) as of (.*)")]
        public void WhenIncreaseQuantityInInventory(int quantity, DateTime asOfUtc)
        {
            _container.Resolve<IncreaseQuantityInInventoryHandler>()
                .Handle(new IncreaseQuantityInInventory(1, 2, _articleId.Id, _stockId.Id, quantity, asOfUtc));
        }

        [When(@"Decrease quantity in inventory of (.*) as of (.*)")]
        public void WhenDecreaseQuantityInInventory(int quantity, DateTime asOfUtc)
        {
            _container.Resolve<DecreaseQuantityInInventoryHandler>()
                .Handle(new DecreaseQuantityInInventory(1, 2, _articleId.Id, _stockId.Id, quantity, asOfUtc));
        }

        [Then(@"quantity in inventory increased of (.*) to (.*) as of (.*)")]
        public void ThenQuantityInInventoryIncreased(int quantity, int total, DateTime asOfUtc)
        {
            var expected = _mutatingEvents.GetNextExpectedEvent<QuantityInInventoryIncreased>();
            Assert.That(expected.StockId, Is.EqualTo(_stockId.Id));
            Assert.That(expected.Quantity, Is.EqualTo(quantity));
            Assert.That(expected.Total, Is.EqualTo(total));
            Assert.That(expected.AsOfUtc, Is.EqualTo(asOfUtc));
        }

        [Then(@"quantity in inventory decreased of (.*) to (.*) as of (.*)")]
        public void ThenQuantityInInventoryDecreased(int quantity, int total, DateTime asOfUtc)
        {
            var expected = _mutatingEvents.GetNextExpectedEvent<QuantityInInventoryDecreased>();
            Assert.That(expected.StockId, Is.EqualTo(_stockId.Id));
            Assert.That(expected.Quantity, Is.EqualTo(quantity));
            Assert.That(expected.Total, Is.EqualTo(total));
            Assert.That(expected.AsOfUtc, Is.EqualTo(asOfUtc));
        }
    }
}