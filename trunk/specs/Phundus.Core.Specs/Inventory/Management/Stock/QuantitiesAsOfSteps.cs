namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using System;
    using Core.Inventory.Domain.Model.Management;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class QuantitiesAsOfSteps
    {
        private QuantitiesAsOf _sut;

        public QuantitiesAsOfSteps()
        {
            _sut = new QuantitiesAsOf();
        }

        [Given(@"I changed quantity of (.*) at (.*)")]
        public void GivenIChangedQuantityOfAt_(int change, DateTime asOfUtc)
        {
            _sut.ChangeAsOf(change, asOfUtc);
        }

        [When(@"I change quantity of (.*) at (.*)")]
        public void WhenIChangeQuantityOfAt_(int change, DateTime asOfUtc)
        {
            _sut.ChangeAsOf(change, asOfUtc);
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
    }
}