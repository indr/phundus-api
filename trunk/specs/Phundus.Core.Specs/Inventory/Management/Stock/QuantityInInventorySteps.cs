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

        //private ArticleId _articleId;
        //private StockId _stockId;
        private StockContext _context;


        public QuantityInInventorySteps(Container container, PastEvents pastEvents, MutatingEvents mutatingEvents, StockContext stockContext)
        {
            _container = container;
            _pastEvents = pastEvents;
            _mutatingEvents = mutatingEvents;
            _context = stockContext;

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
            _context.StockId = new StockId(stockId);
            _context.ArticleId = new ArticleId(1);
            _pastEvents.Add(new StockCreated(_context.StockId.Id, _context.ArticleId.Id));
        }

        [Given(@"quantity in inventory increased of (.*) to (.*) as of (.*)")]
        public void GivenQuantityInInventoryIncreasedOfToAsOf_(int change, int total, DateTime asOfUtc)
        {
            _pastEvents.Add(new QuantityInInventoryIncreased(_context.StockId.Id, change, total, asOfUtc));
        }

        [Given(@"quantity in inventory decreased of (.*) to (.*) as of (.*)")]
        public void GivenQuantityInInventoryDecreasedOfToAsOf_(int change, int total, DateTime asOfUtc)
        {
            _pastEvents.Add(new QuantityInInventoryDecreased(_context.StockId.Id, change, total, asOfUtc));
        }

        [When(@"Increase quantity in inventory of (.*) as of (.*)")]
        public void WhenIncreaseQuantityInInventory(int quantity, DateTime asOfUtc)
        {
            _container.Resolve<IncreaseQuantityInInventoryHandler>()
                .Handle(new IncreaseQuantityInInventory(1, 2, _context.ArticleId.Id, _context.StockId.Id, quantity, asOfUtc));
        }

        [When(@"Decrease quantity in inventory of (.*) as of (.*)")]
        public void WhenDecreaseQuantityInInventory(int quantity, DateTime asOfUtc)
        {
            _container.Resolve<DecreaseQuantityInInventoryHandler>()
                .Handle(new DecreaseQuantityInInventory(1, 2, _context.ArticleId.Id, _context.StockId.Id, quantity, asOfUtc));
        }

        [Then(@"quantity in inventory increased of (.*) to (.*) as of (.*)")]
        public void ThenQuantityInInventoryIncreased(int quantity, int total, DateTime asOfUtc)
        {
            var expected = _mutatingEvents.GetNextExpectedEvent<QuantityInInventoryIncreased>();
            Assert.That(expected.StockId, Is.EqualTo(_context.StockId.Id));
            Assert.That(expected.Change, Is.EqualTo(quantity));
            Assert.That(expected.Total, Is.EqualTo(total));
            Assert.That(expected.AsOfUtc, Is.EqualTo(asOfUtc));
        }

        [Then(@"quantity in inventory decreased of (.*) to (.*) as of (.*)")]
        public void ThenQuantityInInventoryDecreased(int quantity, int total, DateTime asOfUtc)
        {
            var expected = _mutatingEvents.GetNextExpectedEvent<QuantityInInventoryDecreased>();
            Assert.That(expected.StockId, Is.EqualTo(_context.StockId.Id));
            Assert.That(expected.Change, Is.EqualTo(quantity));
            Assert.That(expected.Total, Is.EqualTo(total));
            Assert.That(expected.AsOfUtc, Is.EqualTo(asOfUtc));
        }
    }
}