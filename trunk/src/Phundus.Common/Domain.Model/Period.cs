namespace Phundus.Common.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class Period : ValueObject
    {
        public Period(DateTime fromUtc, DateTime toUtc)
        {
            if (fromUtc > toUtc)
                throw new ArgumentException("The from date must be less or equal the to date.");

            FromUtc = fromUtc;
            ToUtc = toUtc;
        }

        protected Period()
        {
        }

        [DataMember(Order = 1)]
        public DateTime FromUtc { get; protected set; }

        [DataMember(Order = 2)]
        public DateTime ToUtc { get; protected set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FromUtc;
            yield return ToUtc;
        }

        public static Period FromNow(int days)
        {
            var now = DateTime.UtcNow;
            return new Period(now, now.AddDays(days));
        }
    }
}