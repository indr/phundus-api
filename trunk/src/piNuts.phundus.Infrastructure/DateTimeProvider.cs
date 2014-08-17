namespace Phundus.Infrastructure
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

        public static DateTime UtcNow
        {
            get { return _current.UtcNow; }
        }

        public static DateTime UtcToday
        {
            get { return _current.UtcNow.Date; }
        }

        public static void ResetToDefault()
        {
            _current = DefaultDateTimeProvider.Instance;
        }
    }
}