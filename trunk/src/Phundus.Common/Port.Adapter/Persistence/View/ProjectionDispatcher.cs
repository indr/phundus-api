namespace Phundus.Common.Port.Adapter.Persistence.View
{
    using Events;
    using Messaging;
    using Notifications;

    public class ProjectionDispatcher : INotificationConsumer
    {
        private static readonly object Lock = new object();

        public IProcessedNotificationTrackerStore ProcessedNotificationTrackerStore { get; set; }

        public IDomainEventHandlerFactory DomainEventHandlerFactory { get; set; }

        public IEventStore EventStore { get; set; }

        public void Consume(Notification notification)
        {
            // TODO: Remove when MQ is in place
            lock (Lock)
            {
                var domainEventHandler = DomainEventHandlerFactory.GetDomainEventHandlers();

                foreach (var handler in domainEventHandler)
                {
                    UpdateReadModel(handler, notification);
                }
            }
        }


        private void UpdateReadModel(IDomainEventHandler domainEventHandler, Notification notification)
        {
            var tracker =
                ProcessedNotificationTrackerStore.GetProcessedNotificationTracker(domainEventHandler.GetType().FullName);
            if (tracker.MostRecentProcessedNotificationId >= notification.NotificationId)
                return;

            var events = EventStore.GetAllStoredEventsBetween(tracker.MostRecentProcessedNotificationId + 1,
                notification.NotificationId);
            foreach (var each in events)
            {
                domainEventHandler.Handle(EventStore.ToDomainEvent(each));
            }

            ProcessedNotificationTrackerStore.TrackMostRecentProcessedNotification(tracker, notification);
        }
    }
}