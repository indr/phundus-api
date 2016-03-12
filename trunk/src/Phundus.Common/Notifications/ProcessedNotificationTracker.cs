namespace Phundus.Common.Notifications
{
    using System;
    using System.Globalization;

    public class ProcessedNotificationTracker
    {
        private Guid _id;
        private string _typeName;

        public ProcessedNotificationTracker(string typeName)
        {
            if (typeName == null) throw new ArgumentNullException("typeName");
            if (typeName.EndsWith("Proxy", true, CultureInfo.InvariantCulture))
                throw new ArgumentException("The type name must not end with proxy", "typeName");

            _id = Guid.NewGuid();
            _typeName = typeName;
        }

        protected ProcessedNotificationTracker()
        {
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

        public virtual DateTime? MostRecentProcessedAtUtc { get; protected set; }

        public virtual string ErrorMessage { get; protected set; }

        public virtual DateTime? ErrorAtUtc { get; protected set; }

        public virtual void Reset()
        {
            MostRecentProcessedNotificationId = 0;
            MostRecentProcessedAtUtc = DateTimeProvider.UtcNow;
            ErrorMessage = null;
            ErrorAtUtc = null;
        }

        public virtual void Track(long notificationId)
        {
            MostRecentProcessedNotificationId = notificationId;
            MostRecentProcessedAtUtc = DateTimeProvider.UtcNow;
            ErrorMessage = null;
            ErrorAtUtc = null;
        }

        public virtual void Track(Exception exception)
        {
            ErrorAtUtc = DateTimeProvider.UtcNow;
            ErrorMessage = exception.Message;
        }
    }
}