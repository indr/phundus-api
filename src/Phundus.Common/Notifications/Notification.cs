namespace Phundus.Common.Notifications
{
    using System;
    using System.Collections.Generic;
    using Domain.Model;

    public class Notification : ValueObject
    {
        private readonly DomainEvent _domainEvent;
        private readonly long _notificationId;
        private readonly DateTime _occurredOnUtc;
        private readonly string _typeName;
        private readonly int _version;
        private string _streamName;
        private long _streamVersion;

        public Notification(long notificationId, DomainEvent domainEvent, string streamName, long streamVersion)
        {
            _notificationId = notificationId;
            _domainEvent = domainEvent;
            _occurredOnUtc = domainEvent.OccuredOnUtc;
            _version = 0; //domainEvent.EventVersion;
            _typeName = domainEvent.GetType().FullName;
            _streamName = streamName;
            _streamVersion = streamVersion;
        }

        public long NotificationId
        {
            get { return _notificationId; }
        }

        public string StreamName
        {
            get { return _streamName; }
        }

        public long StreamVersion
        {
            get { return _streamVersion; }
        }

        public DateTime OccurredOnUtc
        {
            get { return _occurredOnUtc; }
        }

        public int Version
        {
            get { return _version; }
        }

        public string TypeName
        {
            get { return _typeName; }
        }

        public DomainEvent Event
        {
            get { return _domainEvent; }
        }

        public TEvent GetEvent<TEvent>() where TEvent : DomainEvent
        {
            return (TEvent) _domainEvent;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _notificationId;
        }
    }
}