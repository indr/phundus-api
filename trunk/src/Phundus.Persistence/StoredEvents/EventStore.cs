namespace Phundus.Persistence.StoredEvents
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
            AppendEventStore(domainEvent, new EventStreamId(domainEvent.Id.ToString(), 1), 0);
        }

        public void Append(EventStreamId eventStreamId, IList<IDomainEvent> domainEvents)
        {
            var index = 0;
            foreach (var domainEvent in domainEvents)
            {
                AppendEventStore(domainEvent, eventStreamId, index++);
            }
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

        protected void AppendEventStore(IDomainEvent domainEvent, EventStreamId startingEventStreamId, int index)
        {
            var storedEvent = ToStoredEvent(domainEvent, startingEventStreamId.StreamName,
                startingEventStreamId.StreamVersion + index);
            Repository.Append(storedEvent);

            NotificationPublisher.PublishNotification(storedEvent);
        }

        private StoredEvent ToStoredEvent(IDomainEvent domainEvent, string streamName, long streamVersion)
        {
            var serialization = Serializer.Serialize(domainEvent);

            var domainEventType = domainEvent.GetType();
            var typeName = domainEventType.FullName + ", " + domainEventType.Assembly.GetName().Name;

            var storedEvent = new StoredEvent(domainEvent.Id, domainEvent.OccuredOnUtc,
                typeName, serialization, streamName, streamVersion);

            return storedEvent;
        }        
    }
}