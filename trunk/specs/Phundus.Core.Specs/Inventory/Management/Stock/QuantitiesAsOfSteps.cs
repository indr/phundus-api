namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using System;
    using Common.Domain.Model;
    using Core.Inventory.Domain.Model.Management;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class QuantitiesAsOfSteps
    {
        private readonly QuantitiesAsOf _sut;
        private bool _hasQuantityInPeriod;

        public QuantitiesAsOfSteps()
        {
            _sut = new QuantitiesAsOf();
        }

        [Given(@"I changed quantity of (.*) at (.*)")]
        public void GivenIChangedQuantityOfAt_(int change, DateTime asOfUtc)
        {
            if (change > 0)
                _sut.IncreaseAsOf(change, asOfUtc);
            else
                _sut.DecreaseAsOf(change * -1, asOfUtc);
        }

        [When(@"I change quantity of (.*) at (.*)")]
        public void WhenIChangeQuantityOfAt_(int change, DateTime asOfUtc)
        {
            if (change > 0)
                _sut.IncreaseAsOf(change, asOfUtc);
            else
                _sut.DecreaseAsOf(change * -1, asOfUtc);
        }

        [Then(@"the total as of (.*) should be (.*)")]
        public void ThenTheTotalAsOf_ShouldBe(DateTime asOfUtc, int total)
        {
            var actual = _sut.GetTotalAsOf(asOfUtc);
            Assert.That(actual, Is.EqualTo(total));
        }

        [Then(@"the quantities should be")]
        public void ThenTheQuantitiesShouldBe(Table table)
        {
            var actual = _sut.Quantities;
            table.CompareToSet(actual);
        }

        [When(@"I ask for has quantity in period from (.*) to (.*) of (.*)")]
        public void WhenIAskForHasQuantityInPeriod(DateTime fromUtc, DateTime toUtc, int quantity)
        {
            _hasQuantityInPeriod = _sut.HasQuantityInPeriod(new Period(fromUtc, toUtc), quantity);
        }

        [Then(@"has quantity in period should be (true|false)")]
        public void ThenHasQuantityInPeriodShouldBeFalse(bool value)
        {
            Assert.That(_hasQuantityInPeriod, Is.EqualTo(value));
        }
    }
}