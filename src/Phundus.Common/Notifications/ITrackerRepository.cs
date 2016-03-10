namespace Phundus.Common.Notifications
{
    using System.Collections.Generic;

    public interface ITrackerRepository
    {
        ProcessedNotificationTracker Find(string typeName);
        IList<ProcessedNotificationTracker> GetAll();

        void Save(ProcessedNotificationTracker tracker);
        void Remove(ProcessedNotificationTracker tracker);
    }
}