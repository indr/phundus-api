namespace Phundus.Common.Domain.Model
{
    using System;
    using System.Collections.Generic;

    public class Period : ValueObject
    {
        public Period(DateTime fromUtc, DateTime toUtc)
        {
            AssertionConcern.AssertArgumentGreaterThan(toUtc, fromUtc, "To utc must be greater than from utc.");

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