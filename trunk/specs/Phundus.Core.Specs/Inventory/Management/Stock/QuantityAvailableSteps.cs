namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using System;
    using Core.Inventory.Domain.Model.Management;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class QuantityAvailableSteps
    {
        private readonly StockContext _context;

        public QuantityAvailableSteps(StockContext stockContext)
        {
            _context = stockContext;
        }

        [Then(@"quantity available increased of (.*) to (.*) as of (.*)")]
        public void ThenQuantityAvailableIncreasedOfToAsOf_(int change, int total, DateTime asOfUtc)
        {
            var actual = _context.MutatingEvents.GetNextExpectedEvent<QuantityAvailableChanged>();
            Assert.That(actual.OrganizationId, Is.EqualTo(_context.OrganizationId.Id));
            Assert.That(actual.ArticleId, Is.EqualTo(_context.ArticleId.Id));
            Assert.That(actual.StockId, Is.EqualTo(_context.StockId.Id));
            Assert.That(actual.Change, Is.EqualTo(change));
            Assert.That(actual.Total, Is.EqualTo(total));
            Assert.That(actual.AsOfUtc, Is.EqualTo(asOfUtc));
        }
    }
}