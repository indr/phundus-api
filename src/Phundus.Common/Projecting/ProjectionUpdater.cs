namespace Phundus.Common.Projecting
{
    using System;
    using Castle.Transactions;
    using Eventing;
    using Notifications;

    public interface IProjectionUpdater
    {
        bool Update(IProjection projection);
    }

    public class ProjectionUpdater : IProjectionUpdater
    {
        private readonly IEventStore _eventStore;
        private readonly IProcessedNotificationTrackerStore _trackerStore;

        public ProjectionUpdater(IEventStore eventStore, IProcessedNotificationTrackerStore trackerStore)
        {
            if (eventStore == null) throw new ArgumentNullException("eventStore");
            if (trackerStore == null) throw new ArgumentNullException("trackerStore");
            _eventStore = eventStore;
            _trackerStore = trackerStore;
        }

        public static int NotificationsPerUpdate = 20;

        [Transaction]
        public bool Update(IProjection projection)
        {
            var tracker = _trackerStore.GetProcessedNotificationTracker(projection.GetType().FullName);
            var maxNotificationId = _eventStore.GetMaxNotificationId();
            if (tracker.MostRecentProcessedNotificationId >= maxNotificationId)
                return true;

            var lowNotificationId = tracker.MostRecentProcessedNotificationId + 1;
            var highNotificationId = Math.Min(maxNotificationId, lowNotificationId + NotificationsPerUpdate - 1);

            var storedEvents = _eventStore.AllStoredEventsBetween(lowNotificationId, highNotificationId);
            foreach (var each in storedEvents)
            {
                var e = _eventStore.Deserialize(each);
                projection.Handle(e);
            }

            _trackerStore.TrackMostRecentProcessedNotificationId(tracker, highNotificationId);

            return highNotificationId >= maxNotificationId;
        }
    }
}