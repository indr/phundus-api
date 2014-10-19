namespace Phundus.Common.Notifications
{
    using System.Collections.Generic;

    public interface IProcessedNotificationTrackerStore
    {
        ProcessedNotificationTracker GetProcessedNotificationTracker(string typeName);

        void TrackMostRecentProcessedNotification(ProcessedNotificationTracker tracker,
            IList<Notification> notifications);
    }
}