namespace Phundus.Common.Projecting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Transactions;
    using Eventing;
    using Notifications;

    public interface IProjectionProcessor
    {
        void Update(string typeName);
        void Handle(Notification notification);
        void ProcessMissedNotifications();        
    }

    public class ProjectionProcessor : IProjectionProcessor
    {
        private static object _lock = new object();

        public IProcessedNotificationTrackerStore ProcessedNotificationTrackerStore { get; set; }

        public IDomainEventHandlerFactory DomainEventHandlerFactory { get; set; }

        public IEventStore EventStore { get; set; }

        public void Handle(Notification notification)
        {
            lock (_lock)
            {
                var domainEventHandler = GetDomainEventHandlers();
                foreach (var handler in domainEventHandler)
                {
                    UpdateReadModel(handler, notification);
                }
            }
        }

        [Transaction]
        public void Update(string typeName)
        {
            var domainEventHandler = GetDomainEventHandler(typeName);
            UpdateProjection(domainEventHandler, EventStore.GetMaxNotificationId());
        }

        [Transaction]
        public void ProcessMissedNotifications()
        {
            lock (_lock)
            {
                var domainEventHandler = GetDomainEventHandlers();
                foreach (var handler in domainEventHandler)
                {
                    UpdateProjection(handler, EventStore.GetMaxNotificationId());
                }
            }
        }

        private IStoredEventsConsumer GetDomainEventHandler(string typeName)
        {
            var result = GetDomainEventHandlers().SingleOrDefault(p => p.GetType().FullName == typeName);
            if (result == null)
                throw new Exception("Could not find domain event handler " + typeName);
            return result;
        }

        private IEnumerable<IStoredEventsConsumer> GetDomainEventHandlers()
        {
            return DomainEventHandlerFactory.GetDomainEventHandlers().ToList();
        }

        private void UpdateProjection(IStoredEventsConsumer storedEventsConsumer, long notificationId)
        {
            var tracker =
                ProcessedNotificationTrackerStore.GetProcessedNotificationTracker(
                    storedEventsConsumer.GetType().FullName);
            if (tracker.MostRecentProcessedNotificationId >= notificationId)
                return;

            var events = EventStore.AllStoredEventsBetween(tracker.MostRecentProcessedNotificationId + 1,
                notificationId);
            foreach (var each in events)
            {
                storedEventsConsumer.Handle(EventStore.Deserialize(each));
            }

            ProcessedNotificationTrackerStore.TrackMostRecentProcessedNotificationId(tracker, notificationId);
        }

        private void UpdateReadModel(IStoredEventsConsumer storedEventsConsumer, Notification notification)
        {
            var tracker =
                ProcessedNotificationTrackerStore.GetProcessedNotificationTracker(
                    storedEventsConsumer.GetType().FullName);
            if (tracker.MostRecentProcessedNotificationId >= notification.NotificationId)
                return;

            var events = EventStore.AllStoredEventsBetween(tracker.MostRecentProcessedNotificationId + 1,
                notification.NotificationId);
            foreach (var each in events)
            {
                storedEventsConsumer.Handle(EventStore.Deserialize(each));
            }

            ProcessedNotificationTrackerStore.TrackMostRecentProcessedNotification(tracker, notification);
        }
    }
}