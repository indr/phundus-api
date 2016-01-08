namespace Phundus.Infrastructure
{
    using System;

    public class DefaultDateTimeProvider : IDateTimeProvider
    {
        public static IDateTimeProvider Instance = new DefaultDateTimeProvider();

        public DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }

        public DateTime Today
        {
            get { return DateTime.Today; }
        }
    }
}