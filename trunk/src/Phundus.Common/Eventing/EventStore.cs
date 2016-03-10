namespace Phundus.Common.Eventing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Model;
    using Notifications;

    public interface IEventStore
    {
        void Append(DomainEvent domainEvent);
        void AppendToStream(GuidIdentity id, int version, ICollection<IDomainEvent> events);
        IEnumerable<StoredEvent> AllStoredEventsBetween(long lowStoredEventId, long highStoredEventId);
        long CountStoredEvents();
        long GetMaxNotificationId();
        DomainEvent Deserialize(StoredEvent storedEvent);

        EventStream LoadEventStream(GuidIdentity id);
    }

    public class EventStore : IEventStore
    {
        private readonly IEventSerializer _eventSerializer;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IStoredEventRepository _storedEventRepository;

        public EventStore(INotificationPublisher notificationPublisher, IStoredEventRepository storedEventRepository,
            IEventSerializer eventSerializer)
        {
            if (notificationPublisher == null) throw new ArgumentNullException("notificationPublisher");
            if (storedEventRepository == null) throw new ArgumentNullException("storedEventRepository");
            if (eventSerializer == null) throw new ArgumentNullException("eventSerializer");
            _notificationPublisher = notificationPublisher;
            _storedEventRepository = storedEventRepository;
            _eventSerializer = eventSerializer;
        }


        public void Append(DomainEvent domainEvent)
        {
            var storedEvent = Serialize(domainEvent);
            _storedEventRepository.Append(storedEvent);

            _notificationPublisher.PublishNotification(storedEvent, Deserialize);
        }

        public void AppendToStream(GuidIdentity id, int version, ICollection<IDomainEvent> events)
        {
            var storedEvents = Serialize(events, id.Id, version);
            _storedEventRepository.Append(storedEvents, id.Id, version - 1);

            _notificationPublisher.PublishNotification(storedEvents, Deserialize);
        }

        public IEnumerable<StoredEvent> AllStoredEventsBetween(long lowStoredEventId, long highStoredEventId)
        {
            return _storedEventRepository.AllStoredEventsBetween(lowStoredEventId, highStoredEventId);
        }

        public long CountStoredEvents()
        {
            return _storedEventRepository.CountStoredEvents();
        }

        public long GetMaxNotificationId()
        {
            return _storedEventRepository.GetMaxNotificationId();
        }

        public DomainEvent Deserialize(StoredEvent storedEvent)
        {
            var domainEventType = Type.GetType(storedEvent.TypeName, true);

            try
            {
                return (DomainEvent) _eventSerializer.Deserialize(domainEventType, storedEvent.EventGuid,
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
            var storedEvents = _storedEventRepository.GetStoredEvents(id.Id);
            var domainEvents = new List<IDomainEvent>();
            var version = 0;
            foreach (var storedEvent in storedEvents)
            {
                domainEvents.Add(Deserialize(storedEvent));
                version = storedEvent.Version;
            }

            return new EventStream(domainEvents, version);
        }

        protected StoredEvent Serialize(IDomainEvent domainEvent, Guid? streamId = null, int version = 0)
        {
            var serialization = _eventSerializer.Serialize(domainEvent);

            var domainEventType = domainEvent.GetType();
            var typeName = domainEventType.FullName + ", " + domainEventType.Assembly.GetName().Name;

            var storedEvent = new StoredEvent(domainEvent.EventGuid, domainEvent.OccuredOnUtc,
                typeName, serialization, streamId.HasValue ? streamId.Value : Guid.Empty, version);

            return storedEvent;
        }

        protected ICollection<StoredEvent> Serialize(ICollection<IDomainEvent> domainEvents, Guid streamId, int version)
        {
            return domainEvents.Select(s => Serialize((IDomainEvent) s, streamId, version)).ToList();
        }
    }
}