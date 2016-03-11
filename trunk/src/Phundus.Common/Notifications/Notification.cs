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

        public Notification(long notificationId, DomainEvent domainEvent, int version)
        {
            _notificationId = notificationId;
            _domainEvent = domainEvent;
            _occurredOnUtc = domainEvent.OccuredOnUtc;
            _version = version; //domainEvent.EventVersion;
            _typeName = domainEvent.GetType().FullName;
        }

        public long NotificationId
        {
            get { return _notificationId; }
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

        //public TEvent GetEvent<TEvent>() where TEvent : DomainEvent
        //{
        //    return (TEvent) _domainEvent;
        //}

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _notificationId;
        }
    }
}