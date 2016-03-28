namespace Phundus.Common.Domain.Model
{
    using System;
    using Itenso.TimePeriod;

    public class QuantityPeriod : TimeInterval
    {
        public QuantityPeriod(Period period, int quantity, Guid correlationId = default(Guid))
            : base(period.FromUtc, period.ToUtc, IntervalEdge.Closed, IntervalEdge.Closed, true, false)
        {
            Quantity = quantity;
            CorrelationId = correlationId;
        }

        public QuantityPeriod(DateTime fromUtc, DateTime toUtc, int quantity, Guid correlationId = default(Guid))
            : this(new Period(fromUtc, toUtc), quantity, correlationId)
        {
        }

        public Guid CorrelationId { get; private set; }

        public Period Period
        {
            get { return new Period(Start, End); }
        }

        public int Quantity { get; private set; }

        public void IncQuantity(int quantity)
        {
            Quantity += quantity;
        }
    }
}