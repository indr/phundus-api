namespace Phundus.Common.Notifications
{
    using System;
    using Castle.DynamicProxy;
    using Castle.Transactions;
    using Eventing;

    public interface IStoredEventsProcessor
    {
        bool Process(ISubscribeTo eventSubscriber);
    }

    public class StoredEventsProcessor : IStoredEventsProcessor
    {
        private readonly IEventStore _eventStore;
        private readonly IProcessedNotificationTrackerStore _trackerStore;

        public StoredEventsProcessor(IEventStore eventStore, IProcessedNotificationTrackerStore trackerStore)
        {            
            _eventStore = eventStore;
            _trackerStore = trackerStore;
        }

        public static int NotificationsPerUpdate = 20;

        [Transaction]
        public bool Process(ISubscribeTo eventSubscriber)
        {
            var type = ProxyUtil.GetUnproxiedType(eventSubscriber);
            var tracker = _trackerStore.GetProcessedNotificationTracker(type.FullName);
            var maxNotificationId = _eventStore.GetMaxNotificationId();
            if (tracker.MostRecentProcessedNotificationId >= maxNotificationId)
                return false;

            var lowNotificationId = tracker.MostRecentProcessedNotificationId + 1;
            var highNotificationId = Math.Min(maxNotificationId, lowNotificationId + NotificationsPerUpdate - 1);

            var storedEvents = _eventStore.AllStoredEventsBetween(lowNotificationId, highNotificationId);
            foreach (var each in storedEvents)
            {
                var e = _eventStore.Deserialize(each);
                RedirectToConsume.InvokeEventOptional(eventSubscriber, e);                
            }

            _trackerStore.TrackMostRecentProcessedNotificationId(tracker, highNotificationId);

            return highNotificationId < maxNotificationId;
        }
    }
}