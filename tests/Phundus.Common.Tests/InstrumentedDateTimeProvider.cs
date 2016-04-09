namespace Phundus.Common.Tests
{
    using System;

    public class InstrumentedDateTimeProvider : IDateTimeProvider
    {
        private DateTime _utcNow;

        public InstrumentedDateTimeProvider() : this(DateTime.UtcNow)
        {
        }

        public InstrumentedDateTimeProvider(DateTime utcNow)
        {
            _utcNow = utcNow;
        }

        public DateTime UtcNow
        {
            get { return _utcNow; }
        }

        public DateTime Today
        {
            get
            {
                var local = _utcNow.ToLocalTime();

                return new DateTime(local.Year, local.Month, local.Day, 0, 0, 0, DateTimeKind.Local);
            }
        }

        public void TimePassed(TimeSpan value)
        {
            _utcNow = _utcNow.Add(value);
        }

        public void HoursPassed(int value = 1)
        {
            TimePassed(TimeSpan.FromHours(value));
        }
    }
}