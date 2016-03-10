namespace Phundus.Persistence.Notifications
{
    using System;
    using System.Collections.Generic;
    using Common.Notifications;
    using NHibernate;

    public class NhTrackerRepository : ITrackerRepository
    {
        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get { return SessionFactory(); }
        }

        public ProcessedNotificationTracker Find(string typeName)
        {
            Session.Flush();
            return Session.QueryOver<ProcessedNotificationTracker>()
                .Where(p => p.TypeName == typeName).SingleOrDefault();
        }

        public IList<ProcessedNotificationTracker> GetAll()
        {
            Session.Flush();
            return Session.QueryOver<ProcessedNotificationTracker>()
                .OrderBy(p => p.TypeName).Asc.List();
        }

        public void Save(ProcessedNotificationTracker tracker)
        {
            Session.SaveOrUpdate(tracker);
        }

        public void Remove(ProcessedNotificationTracker tracker)
        {
            Session.Delete(tracker);
        }
    }
}