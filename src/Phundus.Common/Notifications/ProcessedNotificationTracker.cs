namespace Phundus.Common.Notifications
{
    using System;

    public class ProcessedNotificationTracker
    {
        private Guid _id;
        private string _typeName;

        protected ProcessedNotificationTracker()
        {
        }

        public ProcessedNotificationTracker(string typeName)
        {
            _id = Guid.NewGuid();
            _typeName = typeName;
        }

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual int ConcurrencyVersion { get; protected set; }

        public virtual string TypeName
        {
            get { return _typeName; }
            protected set { _typeName = value; }
        }

        public virtual long MostRecentProcessedNotificationId { get; protected set; }

        public virtual DateTime LastProcessingAtUtc { get; protected set; }

        public virtual string ErrorMessage { get; protected set; }

        public virtual void Reset()
        {
            MostRecentProcessedNotificationId = 0;
            //LastProcessingAtUtc = DateTime.UtcNow;
            ErrorMessage = null;
        }

        public virtual void Track(long notificationId)
        {
            MostRecentProcessedNotificationId = notificationId;
            //LastProcessingAtUtc = DateTime.UtcNow;
            ErrorMessage = null;
        }

        public virtual void Track(Exception exception)
        {
            LastProcessingAtUtc = DateTime.UtcNow;
            ErrorMessage = exception.Message;
        }
    }
}