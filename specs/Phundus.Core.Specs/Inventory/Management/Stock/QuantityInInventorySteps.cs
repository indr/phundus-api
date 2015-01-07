namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using System;
    using Contexts;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;
    using NUnit.Framework;
    using Rhino.Mocks;
    using TechTalk.SpecFlow;

    [Binding]
    public class QuantityInInventorySteps
    {
        private readonly Container _container;

        private readonly StockContext _context;
        private readonly MutatingEvents _mutatingEvents;
        private readonly PastEvents _pastEvents;


        public QuantityInInventorySteps(Container container, PastEvents pastEvents, MutatingEvents mutatingEvents,
            StockContext stockContext)
        {
            _container = container;
            _pastEvents = pastEvents;
            _mutatingEvents = mutatingEvents;
            _context = stockContext;

            var repository = _container.Depend.On<IStockRepository>();

            repository.Expect(
                x => x.Get(Arg<OrganizationId>.Is.Anything, Arg<StockId>.Is.Anything))
                .WhenCalled(a => a.ReturnValue = new Stock(pastEvents.Events, 1)).Return(null);

            repository.Expect(x => x.Save(Arg<Stock>.Is.NotNull))
                .WhenCalled(a => _mutatingEvents.Events = ((Stock) a.Arguments[0]).MutatingEvents);
        }

       

        [Given(@"quantity in inventory increased of (.*) to (.*) as of (.*)")]
        public void GivenQuantityInInventoryIncreasedOfToAsOf_(int change, int total, DateTime asOfUtc)
        {
            _pastEvents.Add(new QuantityInInventoryIncreased(_context.OrganizationId, _context.ArticleId,
                _context.StockId, change, total, asOfUtc, null));
        }

        [Given(@"quantity in inventory decreased of (.*) to (.*) as of (.*)")]
        public void GivenQuantityInInventoryDecreasedOfToAsOf_(int change, int total, DateTime asOfUtc)
        {
            _pastEvents.Add(new QuantityInInventoryDecreased(_context.OrganizationId, _context.ArticleId,
                _context.StockId, change, total, asOfUtc, null));
        }

        [When(@"Increase quantity in inventory of (.*) as of (.*)")]
        public void WhenIncreaseQuantityInInventory(int quantity, DateTime asOfUtc)
        {
            _container.Resolve<ChangeQuantityInInventoryHandler>()
                .Handle(new ChangeQuantityInInventory(_context.InitiatorId, _context.OrganizationId, _context.ArticleId,
                    _context.StockId, quantity, asOfUtc, null));
        }

        [When(@"Decrease quantity in inventory of (.*) as of (.*)")]
        public void WhenDecreaseQuantityInInventory(int quantity, DateTime asOfUtc)
        {
            _container.Resolve<ChangeQuantityInInventoryHandler>()
                .Handle(new ChangeQuantityInInventory(_context.InitiatorId, _context.OrganizationId, _context.ArticleId,
                    _context.StockId, quantity*-1, asOfUtc, null));
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