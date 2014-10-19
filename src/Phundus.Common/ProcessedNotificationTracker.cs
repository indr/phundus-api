namespace Phundus.Common
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

        public virtual long MostRecentProcessedNotificationId { get; set; }
    }
}