namespace Phundus.Persistence.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Notifications;
    using NHibernate;

    public class ProcessedNotificationTrackerStore : IProcessedNotificationTrackerStore
    {
        public Func<ISession> SessionFactory { get; set; }

        protected ISession Session
        {
            get { return SessionFactory(); }
        }

        public ProcessedNotificationTracker GetProcessedNotificationTracker(string typeName)
        {
            var tracker = FindProcessedNotificationTracker(typeName);

            if (tracker != null)
                return tracker;

            return new ProcessedNotificationTracker(typeName);
        }

        private ProcessedNotificationTracker FindProcessedNotificationTracker(string typeName)
        {
            var tracker = Session.QueryOver<ProcessedNotificationTracker>()
                .Where(x => x.TypeName == typeName).SingleOrDefault();
            return tracker;
        }

        public IList<ProcessedNotificationTracker> GetProcessedNotificationTrackers()
        {
            return Session.QueryOver<ProcessedNotificationTracker>().OrderBy(p => p.TypeName).Asc.List();
        }

        public void TrackMostRecentProcessedNotification(ProcessedNotificationTracker tracker,
            IList<Notification> notifications)
        {
            var last = notifications.LastOrDefault();
            if (last == null)
                return;

            TrackMostRecentProcessedNotification(tracker, last);
        }

        public void TrackMostRecentProcessedNotification(ProcessedNotificationTracker tracker, Notification notification)
        {
            tracker.Track(notification.NotificationId);            

            Session.SaveOrUpdate(tracker);
        }

        public void TrackMostRecentProcessedNotificationId(ProcessedNotificationTracker tracker, long notificationId)
        {
            tracker.Track(notificationId);

            Session.SaveOrUpdate(tracker);
        }

        public void DeleteTracker(string typeName)
        {
            var tracker = FindProcessedNotificationTracker(typeName);
            if (tracker == null)
                return;

            Session.Delete(tracker);
        }

        public void ResetTracker(string typeName)
        {
            var tracker = FindProcessedNotificationTracker(typeName);
            if (tracker == null)
                return;

            tracker.Reset();

            Session.SaveOrUpdate(tracker);
        }
    }
}