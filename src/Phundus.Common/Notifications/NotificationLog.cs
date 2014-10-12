namespace Phundus.Common.Notifications
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class NotificationLog
    {
        public NotificationLog(string notificationLogId, string nextNotificationLogId, string previousNotificationLogId,
            IEnumerable<Notification> notifications, bool isArchived)
        {
            NotificationLogId = notificationLogId;
            NextNotificationLogId = nextNotificationLogId;
            PreviousNotificationLogId = previousNotificationLogId;
            Notifications = new ReadOnlyCollection<Notification>(notifications.ToArray());
            IsArchived = isArchived;
        }

        public bool IsArchived { get; private set; }

        public ReadOnlyCollection<Notification> Notifications { get; private set; }

        public string PreviousNotificationLogId { get; private set; }

        public string NextNotificationLogId { get; private set; }

        public string NotificationLogId { get; private set; }
    }
}