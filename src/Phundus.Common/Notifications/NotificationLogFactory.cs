namespace Phundus.Common.Notifications
{
    using System.Collections.Generic;
    using System.Linq;
    using Eventing;

    public interface INotificationLogFactory
    {
        NotificationLog CreateCurrentNotificationLog();
        NotificationLog CreateNotificationLog(string notificationLogId);
    }

    public class NotificationLogFactory : INotificationLogFactory
    {
        private const int NotificationsPerLog = 20;        
        private readonly IEventStore _eventStore;

        public NotificationLogFactory(IEventStore eventStore)
        {
            _eventStore = eventStore;            
        }

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
            var max = _eventStore.GetMaxNotificationId();
            if (max == 0)
                max = 1;
            var remainder = max%NotificationsPerLog;

            if (remainder == 0)
            {
                remainder = NotificationsPerLog;
            }

            //long low = count - remainder + 1;
            long low = max - remainder + 1;

            // ensures a minted id value even though there may
            // not be a full set of notifications at present
            long high = low + NotificationsPerLog - 1;

            return new NotificationLogId(low, high);
        }

        private NotificationLog CreateNotificationLog(NotificationLogId notificationLogId)
        {
            var storedEvents = _eventStore.AllStoredEventsBetween(
                notificationLogId.Low, notificationLogId.High);

            // TODO: Bedingt, dass keine Lücken vorhanden sind!
            //var count = EventStore.CountStoredEvents();
            //var archivedIndicator = notificationLogId.High < count;
            var max = _eventStore.GetMaxNotificationId();
            var archivedIndicator = notificationLogId.High < max;

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
            return storedEvents.Select(each =>
                new Notification(each.EventId, _eventStore.Deserialize(each), each.Version))
                .ToList();
        }
    }
}