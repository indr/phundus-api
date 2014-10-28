namespace Phundus.Common.Port.Adapter.Persistence.View
{
    using System;
    using Events;
    using NHibernate;
    using Notifications;

    public class ProjectionDispatcher : INotificationHandler
    {
        private static object _lock = new object();

        public IProcessedNotificationTrackerStore ProcessedNotificationTrackerStore { get; set; }

        public IDomainEventHandlerFactory DomainEventHandlerFactory { get; set; }

        public NHibernateProjectionBase<Object>[] Projectsion { get; set; }

        public IEventStore EventStore { get; set; }

        public void Handle(Notification notification)
        {
            // TODO: Remove when MQ is in place
            lock (_lock)
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