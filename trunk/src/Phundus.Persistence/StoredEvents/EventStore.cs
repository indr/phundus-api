namespace Phundus.Persistence.StoredEvents
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Eventing;
    using Common.Notifications;
    using Ddd;

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

            try
            {
                return (DomainEvent) Serializer.Deserialize(domainEventType, storedEvent.EventGuid,
                    storedEvent.OccuredOnUtc, storedEvent.Serialization);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    String.Format(
                        @"Error deserializing event {0}, event type name ""{1}"", resolved domain event type ""{2}"".",
                        storedEvent.EventGuid, storedEvent.TypeName,
                        domainEventType.FullName + ", " + domainEventType.Assembly.GetName().Name), ex);
            }
        }

        public EventStream LoadEventStream(GuidIdentity id)
        {
            var storedEvents = Repository.GetStoredEvents(id.Id);
            var domainEvents = new List<IDomainEvent>();
            var version = 0;
            foreach (var storedEvent in storedEvents)
            {
                domainEvents.Add(ToDomainEvent(storedEvent));
                version = storedEvent.Version;
            }

            return new EventStream(domainEvents, version);
        }

        protected StoredEvent ToStoredEvent(DomainEvent domainEvent)
        {
            var serialization = Serializer.Serialize(domainEvent);

            var domainEventType = domainEvent.GetType();
            var typeName = domainEventType.FullName + ", " + domainEventType.Assembly.GetName().Name;

            var storedEvent = new StoredEvent(domainEvent.EventGuid, domainEvent.OccuredOnUtc,
                typeName, serialization, Guid.Empty);

            return storedEvent;
        }
    }
}