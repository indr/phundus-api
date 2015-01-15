namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using System;
    using Core.Inventory.Domain.Model.Management;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class QuantityInInventorySteps
    {
        private readonly StockContext _context;

        public QuantityInInventorySteps(StockContext stockContext)
        {
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

        [Then(@"quantity in inventory increased of (.*) to (.*) as of (.*)")]
        public void ThenQuantityInInventoryIncreased(int quantity, int total, DateTime asOfUtc)
        {
            var expected = _context.MutatingEvents.GetNextExpectedEvent<QuantityInInventoryIncreased>();
            Assert.That(expected.StockId, Is.EqualTo(_context.StockId.Id));
            Assert.That(expected.Change, Is.EqualTo(quantity));
            Assert.That(expected.Total, Is.EqualTo(total));
            Assert.That(expected.AsOfUtc, Is.EqualTo(asOfUtc));
        }

        [Then(@"quantity in inventory decreased of (.*) to (.*) as of (.*)")]
        public void ThenQuantityInInventoryDecreased(int quantity, int total, DateTime asOfUtc)
        {
            var expected = _context.MutatingEvents.GetNextExpectedEvent<QuantityInInventoryDecreased>();
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