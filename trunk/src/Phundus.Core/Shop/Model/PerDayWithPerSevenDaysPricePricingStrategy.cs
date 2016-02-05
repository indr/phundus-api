namespace Phundus.Shop.Model
{
    using System;
    using Common.Domain.Model;

    public class PriceInfo
    {
        public int Days { get; set; }
        public decimal Price { get; set; }
    }

    public class PerDayWithPerSevenDaysPricePricingStrategy
    {
        public PriceInfo Calculate(Period perdiod, int quantity, decimal pricePerSevenDays)
        {
            return Calculate(perdiod.FromUtc.ToLocalTime(), perdiod.ToUtc.ToLocalTime(), quantity, pricePerSevenDays);
        }

        public PriceInfo Calculate(DateTime fromLocal, DateTime toLocal, int amount, decimal pricePerSevenDays)
        {
            var totalDays = (toLocal.Date.AddDays(1) - fromLocal.Date).TotalDays;
            var days = (int) Math.Ceiling(totalDays);
            var price = Convert.ToDecimal(days)*amount*pricePerSevenDays/7;

            price = Math.Max(1, Math.Round(price, 0));

            return new PriceInfo
            {
                Days = days,
                Price = price
            };
        }
    }
}