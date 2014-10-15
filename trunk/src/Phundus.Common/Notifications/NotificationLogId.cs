namespace Phundus.Common.Notifications
{
    using System;
    using System.Collections.Generic;
    using Domain.Model;

    public class NotificationLogId : ValueObject
    {
        public NotificationLogId(string notificationLogId)
        {
            var pts = notificationLogId.Split(',');
            Low = long.Parse(pts[0]);
            High = long.Parse(pts[1]);
        }

        public NotificationLogId(long low, long high)
        {
            Low = low;
            High = high;
        }

        public long Low { get; private set; }

        public long High { get; private set; }

        public string Encoded
        {
            get { return Low + "," + High; }
        }

        public static NotificationLogId First(int notificationsPerLog)
        {
            return new NotificationLogId(0, 0).Next(notificationsPerLog);
        }

        public static string GetEncoded(NotificationLogId notificationLogId)
        {
            if (notificationLogId == null)
                return null;

            return notificationLogId.Encoded;
        }

        public NotificationLogId Next(int notificationsPerLog)
        {
            var nextLow = High + 1;
            var nextHigh = nextLow + notificationsPerLog - 1;
            var next = new NotificationLogId(nextLow, nextHigh);
            if (Equals(next))
                next = null;
            return next;
        }

        public NotificationLogId Previous(int notificationsPerLog)
        {
            var previousLow = Math.Max(Low - notificationsPerLog, 1);
            var previousHigh = previousLow + notificationsPerLog - 1;
            var previous = new NotificationLogId(previousLow, previousHigh);
            if (Equals(previous))
                previous = null;
            return previous;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Low;
            yield return High;
        }
    }
}