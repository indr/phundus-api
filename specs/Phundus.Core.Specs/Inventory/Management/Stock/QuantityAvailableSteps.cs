namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using System;
    using Common.Domain.Model;
    using Core.Inventory.Domain.Model.Management;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class QuantityAvailableSteps
    {
        private readonly StockContext _ctx;

        public QuantityAvailableSteps(StockContext stockContext)
        {
            _ctx = stockContext;
        }

        [Given(@"quantity available changed from (.*) of (.*)")]
        public void GivenQuantityAvailableChangedFromOf(DateTime fromUtc, int change)
        {
            _ctx.PastEvents.Add(new QuantityAvailableChanged(_ctx.OrganizationId, _ctx.ArticleId, _ctx.StockId,
                new Period(fromUtc, DateTime.MaxValue), change));
        }

        [Then(@"quantity available changed from (.*) of (.*)")]
        public void ThenQuantityAvailableChangedFromOf(DateTime fromUtc, int change)
        {
            ThenQuantityAvailableChangedFromToOf(fromUtc, DateTime.MaxValue, change);
        }

        [Then(@"quantity available changed from (.*) to (.*) of (.*)")]
        public void ThenQuantityAvailableChangedFromToOf(DateTime fromUtc, DateTime toUtc, int change)
        {
            var actual = _ctx.MutatingEvents.GetExpectedEventOnce<QuantityAvailableChanged>();
            Assert.That(actual.OrganizationId, Is.EqualTo(_ctx.OrganizationId.Id));
            Assert.That(actual.ArticleId, Is.EqualTo(_ctx.ArticleId.Id));
            Assert.That(actual.StockId, Is.EqualTo(_ctx.StockId.Id));
            Assert.That(actual.FromUtc, Is.EqualTo(fromUtc));
            Assert.That(actual.ToUtc, Is.EqualTo(toUtc));
            Assert.That(actual.Change, Is.EqualTo(change));
        }

        [Then(@"quantities available")]
        public void ThenQuantitiesAvailable(Table table)
        {
            var actual = _ctx.Sut.QuantitiesAvailable;
            table.CompareToSet(actual);
        }

    }
}