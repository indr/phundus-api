namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using Common;

    public class QuantityAsOf
    {
        public QuantityAsOf(int change, int total, DateTime asOfUtc)
        {
            AssertionConcern.AssertArgumentNotZero(change, "Change must be greater or less than zero.");
            AssertionConcern.AssertArgumentNotNull(asOfUtc, "As of utc must be provided.");

            Change = change;
            Total = total;
            AsOfUtc = asOfUtc;
        }

        public int Change { get; private set; }
        public int Total { get; private set; }
        public DateTime AsOfUtc { get; private set; }

        public void AddToTotal(int change)
        {
            AssertionConcern.AssertArgumentNotZero(change, "Change must be greather or less than zero.");

            Total += change;
        }
    }
}