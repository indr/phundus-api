namespace Phundus.Ddd
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Events;
    using Common.Notifications;
    using Core.Ddd;

    public class EventStore : IEventStore
    {
        public IEventSerializer Serializer { get; set; }

        public IStoredEventRepository Repository { get; set; }

        public INotificationPublisher NotificationPublisher { get; set; }

        public void Append(DomainEvent domainEvent)
        {
            var storedEvent = ToStoredEvent(domainEvent);
            Repository.Append(storedEvent);

            NotificationPublisher.PublishNotification(storedEvent);
        }

        public IEnumerable<StoredEvent> AllStoredEventsBetween(long lowStoredEventId, long highStoredEventId)
        {
            return Repository.AllStoredEventsBetween(lowStoredEventId, highStoredEventId);
        }

        public long CountStoredEvents()
        {
            return Repository.CountStoredEvents();
        }

        public long GetMaxNotificationId()
        {
            return Repository.GetMaxNotificationId();
        }

        public DomainEvent ToDomainEvent(StoredEvent storedEvent)
        {
            var domainEventType = Type.GetType(storedEvent.TypeName, true);

            return (DomainEvent) Serializer.Deserialize(domainEventType, storedEvent.EventGuid,
                storedEvent.OccuredOnUtc, storedEvent.Serialization);
        }

        protected StoredEvent ToStoredEvent(DomainEvent domainEvent)
        {
            var serialization = Serializer.Serialize(domainEvent);

            var domainEventType = domainEvent.GetType();
            var typeName = domainEventType.FullName + ", " + domainEventType.Assembly.GetName().Name;

            var storedEvent = new StoredEvent(domainEvent.Id, domainEvent.OccuredOnUtc,
                typeName, serialization);

            return storedEvent;
        }
    }
}