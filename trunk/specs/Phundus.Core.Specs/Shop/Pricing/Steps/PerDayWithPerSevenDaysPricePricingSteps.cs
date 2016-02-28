namespace Phundus.Core.Specs.Shop.Pricing.Steps
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Phundus.Shop.Model;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class PerDayWithPerSevenDaysPricePricingSteps
    {
        private readonly IList<PriceInfo> _calculatedPrices = new List<PriceInfo>();
        private decimal _unitPricePerWeek;

        [Given(@"a per week price of (.*)")]
        public void GivenAPerWeekPriceOf_CHF(decimal price)
        {
            _unitPricePerWeek = price;
        }

        [When(@"I calculate the per day price with these values")]
        public void WhenICalculateThePerDayPriceWithTheseValues(Table table)
        {
            foreach (var each in table.Rows)
            {
                var fromLocal = Convert.ToDateTime(each["FromLocal"]);
                fromLocal = new DateTime(fromLocal.Ticks, DateTimeKind.Local);
                
                var toLocal = Convert.ToDateTime(each["ToLocal"]);
                toLocal = new DateTime(toLocal.Ticks, DateTimeKind.Local);
                
                var fromUtc = fromLocal.ToUniversalTime();
                var toUtc = toLocal.ToUniversalTime();
                var quantity = Convert.ToInt32(each["Quantity"]);

                _calculatedPrices.Add(new PerDayWithPerSevenDaysPricePricingStrategy().Calculate(new Period(fromUtc, toUtc), quantity,
                    _unitPricePerWeek));
            }
        }

        [Then(@"the resulting prices should be")]
        public void ThenTheResultingPricesShouldBe(Table table)
        {
            table.CompareToSet(_calculatedPrices);
        }
    }
}