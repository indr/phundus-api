namespace Phundus.Common.Domain.Model
{
    using System;
    using System.Collections.Generic;

    public class TimeRange : ValueObject
    {
        public TimeRange(DateTime fromUtc, DateTime toUtc)
        {
            FromUtc = fromUtc;
            ToUtc = toUtc;
        }

        public DateTime FromUtc { get; private set; }

        public DateTime ToUtc { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FromUtc;
            yield return ToUtc;
        }
    }
}