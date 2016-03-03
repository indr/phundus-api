namespace Phundus.Common.Projecting
{
    using System;
    using Castle.Transactions;
    using Eventing;
    using Notifications;

    public interface IProjectionUpdater
    {
        void Update(IProjection projection);
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

        [Transaction]
        public void Update(IProjection projection)
        {
            var tracker = _trackerStore.GetProcessedNotificationTracker(projection.GetType().FullName);
            var maxNotificationId = _eventStore.GetMaxNotificationId();
            var storedEvents = _eventStore.AllStoredEventsBetween(tracker.MostRecentProcessedNotificationId + 1,
                maxNotificationId);

            foreach (var each in storedEvents)
            {
                var e = _eventStore.Deserialize(each);
                projection.Handle(e);
            }

            _trackerStore.TrackMostRecentProcessedNotificationId(tracker, maxNotificationId);
        }
    }
}