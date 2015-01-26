namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using System;
    using Contexts;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Management;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class QuantityInInventorySteps
    {
        private readonly Container _container;
        private readonly StockContext _context;

        public QuantityInInventorySteps(Container container, StockContext stockContext)
        {
            _container = container;
            _context = stockContext;
        }

        [Given(@"quantity in inventory increased of (.*) to (.*) as of (.*)")]
        public void GivenQuantityInInventoryIncreasedOfToAsOf_(int change, int total, DateTime asOfUtc)
        {
            _context.PastEvents.Add(new QuantityInInventoryIncreased(_context.OrganizationId, _context.ArticleId,
                _context.StockId, change, total, asOfUtc, null));
        }

        [Given(@"quantity in inventory decreased of (.*) to (.*) as of (.*)")]
        public void GivenQuantityInInventoryDecreasedOfToAsOf_(int change, int total, DateTime asOfUtc)
        {
            _context.PastEvents.Add(new QuantityInInventoryDecreased(_context.OrganizationId, _context.ArticleId,
                _context.StockId, change, total, asOfUtc, null));
        }

        [When(@"increase quantity in inventory of (.*) as of (.*)")]
        public void WhenIncreaseQuantityInInventory(int quantity, DateTime asOfUtc)
        {
            _container.Resolve<ChangeQuantityInInventoryHandler>()
                .Handle(new ChangeQuantityInInventory(_context.InitiatorId, _context.OrganizationId, _context.ArticleId,
                    _context.StockId, quantity, asOfUtc, null));
        }

        [When(@"decrease quantity in inventory of (.*) as of (.*)")]
        public void WhenDecreaseQuantityInInventory(int quantity, DateTime asOfUtc)
        {
            _container.Resolve<ChangeQuantityInInventoryHandler>()
                .Handle(new ChangeQuantityInInventory(_context.InitiatorId, _context.OrganizationId, _context.ArticleId,
                    _context.StockId, quantity * -1, asOfUtc, null));
        }

        [Then(@"quantity in inventory increased of (.*) to (.*) as of (.*)")]
        public void ThenQuantityInInventoryIncreased(int quantity, int total, DateTime asOfUtc)
        {
            var expected = _context.MutatingEvents.GetExpectedEventOnce<QuantityInInventoryIncreased>();
            Assert.That(expected.StockId, Is.EqualTo(_context.StockId.Id));
            Assert.That(expected.Change, Is.EqualTo(quantity));
            Assert.That(expected.Total, Is.EqualTo(total));
            Assert.That(expected.AsOfUtc, Is.EqualTo(asOfUtc));
        }

        [Then(@"quantity in inventory decreased of (.*) to (.*) as of (.*)")]
        public void ThenQuantityInInventoryDecreased(int quantity, int total, DateTime asOfUtc)
        {
            var expected = _context.MutatingEvents.GetExpectedEventOnce<QuantityInInventoryDecreased>();
            Assert.That(expected.StockId, Is.EqualTo(_context.StockId.Id));
            Assert.That(expected.Change, Is.EqualTo(quantity));
            Assert.That(expected.Total, Is.EqualTo(total));
            Assert.That(expected.AsOfUtc, Is.EqualTo(asOfUtc));
        }

        [Then(@"quantities in inventory")]
        public void ThenQuantitiesInInventory(Table table)
        {
            var actual = _context.Sut.QuantitiesInInventory;
            table.CompareToSet(actual);
        }

    }
}