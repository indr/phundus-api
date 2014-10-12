namespace Phundus.Common.Notifications
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Model;
    using Events;

    public class NotificationLogFactory : INotificationLogFactory
    {
        private const int NotificationsPerLog = 5;

        public IEventStore EventStore { get; set; }

        public IEventSerializer EventSerializer { get; set; }

        public NotificationLog CreateCurrentNotificationLog()
        {
            return CreateNotificationLog(CalculateCurrentNotificationLogId());
        }

        public NotificationLog CreateNotificationLog(string notificationLogId)
        {
            return CreateNotificationLog(new NotificationLogId(notificationLogId));
        }

        private NotificationLogId CalculateCurrentNotificationLogId()
        {
            long count = EventStore.CountStoredEvents();

            long remainder = count%NotificationsPerLog;

            if (remainder == 0)
            {
                remainder = NotificationsPerLog;
            }

            long low = count - remainder + 1;

            // ensures a minted id value even though there may
            // not be a full set of notifications at present
            long high = low + NotificationsPerLog - 1;

            return new NotificationLogId(low, high);
        }

        private NotificationLog CreateNotificationLog(NotificationLogId notificationLogId)
        {
            var storedEvents = EventStore.AllStoredEventsBetween(
                notificationLogId.Low, notificationLogId.High);

            long count = EventStore.CountStoredEvents();

            bool archivedIndicator = notificationLogId.High < count;

            var notificationLog = new NotificationLog(
                notificationLogId.Encoded,
                NotificationLogId.GetEncoded(notificationLogId.Next(NotificationsPerLog)),
                NotificationLogId.GetEncoded(notificationLogId.Previous(NotificationsPerLog)),
                GetNotificationsFrom(storedEvents),
                archivedIndicator);

            return notificationLog;
        }

        private IEnumerable<Notification> GetNotificationsFrom(IEnumerable<StoredEvent> storedEvents)
        {
            return storedEvents.Select(each => new Notification(each.EventId, EventStore.ToDomainEvent(each))).ToList();
        }
    }
}