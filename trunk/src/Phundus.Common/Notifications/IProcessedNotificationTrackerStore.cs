namespace Phundus.Common.Notifications
{
    using System;
    using System.Collections.Generic;

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
}