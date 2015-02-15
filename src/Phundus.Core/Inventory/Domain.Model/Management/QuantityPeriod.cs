namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using Common.Domain.Model;

    public class QuantityPeriod
    {
        public QuantityPeriod(Period period, int quantity)
        {
            Period = period;
            Quantity = quantity;
        }

        public Period Period { get; private set; }

        public DateTime FromUtc { get { return Period.FromUtc; } }

        public DateTime ToUtc { get { return Period.ToUtc; } }

        public int Quantity { get; private set; }

        public bool IsInPeriod(DateTime utc)
        {
            return (utc >= Period.FromUtc) && (utc <= Period.ToUtc);
        }

        public override string ToString()
        {
            return String.Format("QuantityPeriod [Period={0}, Quantity={1}]", Period, Quantity);
        }
    }
}