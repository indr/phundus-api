namespace Phundus.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Notifications;
    using FluentNHibernate.Mapping;
    using NHibernate;
    using Remotion.Linq.Clauses.ResultOperators;

    public class ProcessedNotificationTrackerMap : ClassMap<ProcessedNotificationTracker>
    {
        public ProcessedNotificationTrackerMap()
        {
            SchemaAction.None();

            Id(x => x.Id, "TrackerId").GeneratedBy.Assigned();
            Version(x => x.ConcurrencyVersion);

            Map(x => x.TypeName);
            Map(x => x.MostRecentProcessedNotificationId);
        }
    }

    public class NHibernateProcessedNotificationTrackerStore : IProcessedNotificationTrackerStore
    {
        public Func<ISession> SessionFactory { get; set; }

        protected ISession Session
        {
            get { return SessionFactory(); }
        }

        public ProcessedNotificationTracker GetProcessedNotificationTracker(string typeName)
        {
            var tracker = Session.QueryOver<ProcessedNotificationTracker>()
                .Where(x => x.TypeName == typeName).SingleOrDefault();

            if (tracker != null)
                return tracker;

            return new ProcessedNotificationTracker(typeName);
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
            tracker.MostRecentProcessedNotificationId = notification.NotificationId;

            Session.SaveOrUpdate(tracker);
        }
    }
}