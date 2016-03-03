namespace Phundus.Common
{
    using System;

    public abstract class DateTimeProvider
    {
        private static IDateTimeProvider _current = DefaultDateTimeProvider.Instance;

        public static IDateTimeProvider Current
        {
            get { return _current; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _current = value;
            }
        }

        public static void Set(IDateTimeProvider provider)
        {
            Current = provider;
        }

        public static DateTime UtcNow
        {
            get { return _current.UtcNow; }
        }

        public static DateTime UtcToday
        {
            get { return _current.UtcNow.Date; }
        }

        public static DateTime UtcYesterday
        {
            get { return _current.UtcNow.Date.AddDays(-1); }
        }

        public static DateTime Today
        {
            get { return _current.Today; }
        }

        public static void ResetToDefault()
        {
            _current = DefaultDateTimeProvider.Instance;
        }
    }
}