namespace Phundus.Common.Notifications
{
    using System.Collections.Generic;

    public interface IProcessedNotificationTrackerStore
    {
        ProcessedNotificationTracker GetProcessedNotificationTracker(string typeName);
        IList<ProcessedNotificationTracker> GetProcessedNotificationTrackers();

        void TrackMostRecentProcessedNotification(ProcessedNotificationTracker tracker,
            IList<Notification> notifications);

        void TrackMostRecentProcessedNotification(ProcessedNotificationTracker tracker, Notification notification);
        void TrackMostRecentProcessedNotificationId(ProcessedNotificationTracker tracker, long notificationId);
        void DeleteTracker(string typeName);
    }
}