namespace Phundus.Core.Shop.Pricing.Model
{
    using System;

    public class PriceInfo
    {
        public int Days { get; set; }
        public decimal Price { get; set; }
    }

    public class PerDayWithPerSevenDaysPricePricingStrategy
    {
        public PriceInfo Calculate(DateTime fromLocal, DateTime toLocal, int amount, decimal pricePerSevenDays)
        {
            var totalDays = (toLocal.Date.AddDays(1) - fromLocal.Date).TotalDays;
            var days = (int) Math.Ceiling(totalDays);
            

            //const int secondsPerDay = 60*60*24;
            //var days = Convert.ToInt32(1 + Math.Floor((toLocal - fromLocal).TotalSeconds/secondsPerDay));

            var prices = Convert.ToDecimal(days)*amount*pricePerSevenDays/7;

            return new PriceInfo
            {
                Days = days,
                Price = prices
            };
        }
    }
}