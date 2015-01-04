namespace Phundus.Common.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        public static DateTime ToLocalStartOfTheDayInUtc(this DateTime dateTime)
        {
            return dateTime.ToLocalTime().Date.ToUniversalTime();
        }

        public static DateTime ToLocalEndOfTheDayInUtc(this DateTime dateTime)
        {
            return dateTime.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime();
        }
    }
}