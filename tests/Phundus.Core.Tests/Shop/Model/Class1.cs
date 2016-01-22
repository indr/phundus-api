namespace Phundus.Tests.Shop.Model
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Phundus.Shop.Pricing.Model;

    [Subject(typeof (PerDayWithPerSevenDaysPricePricingStrategy))]
    public class test : Observes<PerDayWithPerSevenDaysPricePricingStrategy>
    {
        private static Period thePeriod;
        private static int theQuantity;
        private static decimal thePricePerSevenDays;
        private static PriceInfo priceInfo;

        private Establish ctx = () =>
        {
            thePeriod = new Period(DateTime.Today.ToUniversalTime(),
                DateTime.Today.AddDays(3).AddSeconds(-1).ToUniversalTime());
            theQuantity = 1;
            thePricePerSevenDays = 7;
        };

        private Because of = () =>
            priceInfo = sut.Calculate(thePeriod, theQuantity, thePricePerSevenDays);

        private It should_have_days_equal_3 = () =>
            priceInfo.Days.ShouldEqual(3);

        private It should_have_price_equal_3_00m = () =>
            priceInfo.Price.ShouldEqual(3.00m);

    }
}