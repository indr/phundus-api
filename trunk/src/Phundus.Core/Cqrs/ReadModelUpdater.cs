namespace Phundus.Dashboard.Querying
{
    using Castle.Transactions;
    using Common.Events;
    using Common.Notifications;

    public class ReadModelUpdater : INotificationHandler
    {
        private static object _lock = new object();

        public IProcessedNotificationTrackerStore ProcessedNotificationTrackerStore { get; set; }

        public IDomainEventHandlerFactory DomainEventHandlerFactory { get; set; }

        public IEventStore EventStore { get; set; }

        public void Handle(Notification notification)
        {
            lock (_lock)
            {
                var domainEventHandler = DomainEventHandlerFactory.GetDomainEventHandlers();

                foreach (var handler in domainEventHandler)
                {
                    UpdateReadModel(handler, notification);
                }
            }
        }

        [Transaction]
        public void ProcessMissedNotifications()
        {
            lock (_lock)
            {
                var domainEventHandler = DomainEventHandlerFactory.GetDomainEventHandlers();
                foreach (var handler in domainEventHandler)
                {
                    UpdateReadModel(handler, EventStore.GetMaxNotificationId());
                }
            }
        }

        private void UpdateReadModel(IDomainEventHandler domainEventHandler, long notificationId)
        {
            
            var tracker =
                ProcessedNotificationTrackerStore.GetProcessedNotificationTracker(domainEventHandler.GetType().FullName);
            if (tracker.MostRecentProcessedNotificationId >= notificationId)
                return;

            var events = EventStore.AllStoredEventsBetween(tracker.MostRecentProcessedNotificationId + 1,
                notificationId);
            foreach (var each in events)
            {
                domainEventHandler.Handle(EventStore.ToDomainEvent(each));
            }

            ProcessedNotificationTrackerStore.TrackMostRecentProcessedNotificationId(tracker, notificationId);
        }

        private void UpdateReadModel(IDomainEventHandler domainEventHandler, Notification notification)
        {
            var tracker =
                ProcessedNotificationTrackerStore.GetProcessedNotificationTracker(domainEventHandler.GetType().FullName);
            if (tracker.MostRecentProcessedNotificationId >= notification.NotificationId)
                return;

            var events = EventStore.AllStoredEventsBetween(tracker.MostRecentProcessedNotificationId + 1,
                notification.NotificationId);
            foreach (var each in events)
            {
                domainEventHandler.Handle(EventStore.ToDomainEvent(each));
            }

            ProcessedNotificationTrackerStore.TrackMostRecentProcessedNotification(tracker, notification);
        }
    }
}