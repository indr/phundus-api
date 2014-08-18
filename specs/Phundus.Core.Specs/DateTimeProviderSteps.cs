namespace Phundus.Core.Specs
{
    using System;
    using Infrastructure;
    using TechTalk.SpecFlow;

    public class InstrumentedDateTimeProvider : IDateTimeProvider
    {
        private readonly DateTime _utcNow;

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
    }

    [Binding]
    public class DateTimeProviderSteps
    {
        [Given(@"today is (\d+)\.(\d+)\.(\d+)")]
        public void GivenTodayIs_(int day, int month, int year, int hour, int minute, int second)
        {
            var utcNow = DateTime.UtcNow;
            DateTimeProvider.Current = new InstrumentedDateTimeProvider(
                new DateTime(year, month, day, utcNow.Hour, utcNow.Minute, utcNow.Second, DateTimeKind.Utc));
        }

        [Given(@"now is (\d+)\.(\d+)\.(\d+) (\d+):(\d+):(\d+)")]
        public void GivenNowIs_(int day, int month, int year, int hour, int minute, int second)
        {
            var utcNow = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
            DateTimeProvider.Current = new InstrumentedDateTimeProvider(utcNow);
        }

        [After]
        public void ResetTimeProvider()
        {
            DateTimeProvider.ResetToDefault();
        }
    }
}