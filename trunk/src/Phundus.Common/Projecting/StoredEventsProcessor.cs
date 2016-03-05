namespace Phundus.Common.Projecting
{
    using System;
    using Castle.Transactions;
    using Eventing;
    using Notifications;

    public interface IStoredEventsProcessor
    {
        bool Process(IEventConsumer consumer);
    }

    public class StoredEventsProcessor : IStoredEventsProcessor
    {
        private readonly IEventStore _eventStore;
        private readonly IProcessedNotificationTrackerStore _trackerStore;

        public StoredEventsProcessor(IEventStore eventStore, IProcessedNotificationTrackerStore trackerStore)
        {
            if (eventStore == null) throw new ArgumentNullException("eventStore");
            if (trackerStore == null) throw new ArgumentNullException("trackerStore");
            _eventStore = eventStore;
            _trackerStore = trackerStore;
        }

        public static int NotificationsPerUpdate = 20;

        [Transaction]
        public bool Process(IEventConsumer consumer)
        {
            var tracker = _trackerStore.GetProcessedNotificationTracker(consumer.GetType().FullName);
            var maxNotificationId = _eventStore.GetMaxNotificationId();
            if (tracker.MostRecentProcessedNotificationId >= maxNotificationId)
                return false;

            var lowNotificationId = tracker.MostRecentProcessedNotificationId + 1;
            var highNotificationId = Math.Min(maxNotificationId, lowNotificationId + NotificationsPerUpdate - 1);

            var storedEvents = _eventStore.AllStoredEventsBetween(lowNotificationId, highNotificationId);
            foreach (var each in storedEvents)
            {
                var e = _eventStore.Deserialize(each);
                RedirectToConsume.InvokeEventOptional(consumer, e);                
            }

            _trackerStore.TrackMostRecentProcessedNotificationId(tracker, highNotificationId);

            return highNotificationId < maxNotificationId;
        }
    }
}