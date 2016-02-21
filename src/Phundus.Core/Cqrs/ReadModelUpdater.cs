namespace Phundus.Cqrs
{
    using System.Collections.Generic;
    using System.Linq;
    using Castle.MicroKernel;
    using Castle.Transactions;
    using Common.Events;
    using Common.Notifications;
    using NHibernate.Mapping;
    using Shop.Projections;

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
                var domainEventHandler = GetDomainEventHandlers();
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
                var domainEventHandler = GetDomainEventHandlers();
                foreach (var handler in domainEventHandler)
                {
                    UpdateReadModel(handler, EventStore.GetMaxNotificationId());
                }
            }
        }

        private IEnumerable<IStoredEventsConsumer> GetDomainEventHandlers()
        {
            // Because of foreign key constraints, we need to update some projections first.
            // This is totally annoying. I don't know how to tell SchemaAction.All to
            // not create foreign keys...

            var handlers = DomainEventHandlerFactory.GetDomainEventHandlers().ToList();

            var result = new List<IStoredEventsConsumer>();
            result.Add(handlers.Single(p => p.GetType() == typeof(ResultItemsProjection)));
            result.Add(handlers.Single(p => p.GetType() == typeof(ShopItemProjection)));

            foreach (var each in result)
            {
                handlers.Remove(each);
            }
            result.AddRange(handlers);

            return result;
        }

        private void UpdateReadModel(IStoredEventsConsumer storedEventsConsumer, long notificationId)
        {
            var tracker =
                ProcessedNotificationTrackerStore.GetProcessedNotificationTracker(
                    storedEventsConsumer.GetType().FullName);
            if (tracker.MostRecentProcessedNotificationId >= notificationId)
                return;

            var events = EventStore.AllStoredEventsBetween(tracker.MostRecentProcessedNotificationId + 1,
                notificationId).OrderBy(p => p.OccuredOnUtc).ThenBy(p => p.EventId);
            foreach (var each in events)
            {
                storedEventsConsumer.Handle(EventStore.ToDomainEvent(each));
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
                storedEventsConsumer.Handle(EventStore.ToDomainEvent(each));
            }

            ProcessedNotificationTrackerStore.TrackMostRecentProcessedNotification(tracker, notification);
        }
    }
}