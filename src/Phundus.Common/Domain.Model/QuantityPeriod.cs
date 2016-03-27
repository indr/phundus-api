namespace Phundus.Common.Domain.Model
{
    using System;
    using System.Runtime.Serialization;
    using Itenso.TimePeriod;

    [DataContract]
    public class QuantityPeriod : TimeInterval
    {
        public QuantityPeriod(Period period, int quantity)
            : base(period.FromUtc, period.ToUtc, IntervalEdge.Closed, IntervalEdge.Closed, true, false)
        {
            Quantity = quantity;
        }

        [Obsolete]
        public QuantityPeriod(ITimePeriod period, int quantity) : base(period)
        {
            Quantity = quantity;
        }

        public QuantityPeriod(DateTime fromUtc, DateTime toUtc, int quantity)
            : this(new Period(fromUtc, toUtc), quantity)
        {
        }

        [DataMember(Order = 1)]
        public Period Period
        {
            get { return new Period(Start, End); }
            set
            {
                base.ExpandStartTo(value.FromUtc);
                base.ExpandStartTo(value.ToUtc);
                //Start = value.FromUtc;
                //End = value.ToUtc;
            }
        }

        [DataMember(Order = 2)]
        public int Quantity { get; protected set; }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }
    }
}