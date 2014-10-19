namespace Phundus.Common
{
    using System;

    public class ProcessedNotificationTracker
    {
        public virtual Guid Id { get; protected set; }
        public virtual int ConcurrencyVersion { get; protected set; }
        public virtual string TypeName { get; protected set; }
        public virtual long MostRecentProcessedNotificationId { get; set; }
    }
}