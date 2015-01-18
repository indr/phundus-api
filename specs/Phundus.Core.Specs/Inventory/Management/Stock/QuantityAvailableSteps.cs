namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using System;
    using Core.Inventory.Domain.Model.Management;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class QuantityAvailableSteps
    {
        private readonly StockContext _context;

        public QuantityAvailableSteps(StockContext stockContext)
        {
            _context = stockContext;
        }

        [Then(@"quantity available changed from (.*) of (.*)")]
        public void ThenQuantityAvailableChangedFromOf(DateTime fromUtc, int change)
        {
            ThenQuantityAvailableChangedFromToOf(fromUtc, DateTime.MaxValue, change);
        }

        [Then(@"quantity available changed from (.*) to (.*) of (.*)")]
        public void ThenQuantityAvailableChangedFromToOf(DateTime fromUtc, DateTime toUtc, int change)
        {
            var actual = _context.MutatingEvents.GetNextExpectedEvent<QuantityAvailableChanged>();
            Assert.That(actual.OrganizationId, Is.EqualTo(_context.OrganizationId.Id));
            Assert.That(actual.ArticleId, Is.EqualTo(_context.ArticleId.Id));
            Assert.That(actual.StockId, Is.EqualTo(_context.StockId.Id));
            Assert.That(actual.FromUtc, Is.EqualTo(fromUtc));
            Assert.That(actual.ToUtc, Is.EqualTo(toUtc));
            Assert.That(actual.Change, Is.EqualTo(change));
        }

        [Then(@"quantities available")]
        public void ThenQuantitiesAvailable(Table table)
        {
            var actual = _context.Sut.QuantitiesAvailable;
            table.CompareToSet(actual);
        }

    }
}