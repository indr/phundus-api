namespace Phundus.Persistence.Notifications
{
    using System;
    using System.Collections.Generic;
    using Castle.Transactions;
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

        public IList<ProcessedNotificationTracker> GetProcessedNotificationTrackers()
        {
            return Session.QueryOver<ProcessedNotificationTracker>().OrderBy(p => p.TypeName).Asc.List();
        }

        [Transaction]
        public void TrackException(string typeName, Exception ex)
        {
            var tracker = GetProcessedNotificationTracker(typeName);
            tracker.Track(ex);
            Session.SaveOrUpdate(tracker);
        }

        public void TrackMostRecentProcessedNotification(ProcessedNotificationTracker tracker, Notification notification)
        {
            TrackMostRecentProcessedNotificationId(tracker, notification.NotificationId);
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

        private ProcessedNotificationTracker FindProcessedNotificationTracker(string typeName)
        {
            var tracker = Session.QueryOver<ProcessedNotificationTracker>()
                .Where(x => x.TypeName == typeName).SingleOrDefault();
            return tracker;
        }
    }
}