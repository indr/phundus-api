namespace Phundus.Common.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Castle.Transactions;

    public interface IProcessedNotificationTrackerStore
    {
        ProcessedNotificationTracker GetProcessedNotificationTracker(string typeName);
        IList<ProcessedNotificationTracker> GetProcessedNotificationTrackers();

        void TrackException(string typeName, Exception ex);
        void TrackMostRecentProcessedNotification(ProcessedNotificationTracker tracker, Notification notification);
        void TrackMostRecentProcessedNotificationId(ProcessedNotificationTracker tracker, long notificationId);

        void DeleteTracker(string typeName);
        void ResetTracker(string typeName);
    }

    public class ProcessedNotificationTrackerStore : IProcessedNotificationTrackerStore
    {
        private readonly ITrackerRepository _trackerRepository;

        public ProcessedNotificationTrackerStore(ITrackerRepository trackerRepository)
        {
            _trackerRepository = trackerRepository;
        }

        public ProcessedNotificationTracker GetProcessedNotificationTracker(string typeName)
        {
            var tracker = FindProcessedNotificationTracker(typeName);

            if (tracker != null)
                return tracker;

            return new ProcessedNotificationTracker(typeName);
        }

        public IList<ProcessedNotificationTracker> GetProcessedNotificationTrackers()
        {
            return _trackerRepository.GetAll();
        }

        [Transaction]
        public void TrackException(string typeName, Exception ex)
        {
            var tracker = GetProcessedNotificationTracker(typeName);
            tracker.Track(ex);

            _trackerRepository.Save(tracker);
        }

        public void TrackMostRecentProcessedNotification(ProcessedNotificationTracker tracker, Notification notification)
        {
            TrackMostRecentProcessedNotificationId(tracker, notification.NotificationId);
        }

        public void TrackMostRecentProcessedNotificationId(ProcessedNotificationTracker tracker, long notificationId)
        {
            tracker.Track(notificationId);

            _trackerRepository.Save(tracker);
        }

        public void DeleteTracker(string typeName)
        {
            var tracker = FindProcessedNotificationTracker(typeName);
            if (tracker == null)
                return;

            _trackerRepository.Remove(tracker);
        }

        public void ResetTracker(string typeName)
        {
            var tracker = FindProcessedNotificationTracker(typeName);
            if (tracker == null)
                return;

            tracker.Reset();

            _trackerRepository.Save(tracker);
        }

        private ProcessedNotificationTracker FindProcessedNotificationTracker(string typeName)
        {
            if (typeName.EndsWith("Proxy", true, CultureInfo.InvariantCulture))
                throw new ArgumentException(
                    "Trying to find a tracker that ends with proxy. Use ProxyUtil to get the underlying type.",
                    "typeName");
            return _trackerRepository.Find(typeName);
        }
    }
}