namespace Phundus.Persistence.StoredEvents
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Events;
    using Common.Messaging;
    using Core.Ddd;
    using NHibernate;

    public class EventStore : IEventStore
    {
        public EventStore(INotificationProducer notificationProducer)
        {
            NotificationProducer = notificationProducer;
        }

        public Func<ISession> SessionFactory { get; set; }

        protected ISession Session
        {
            get { return SessionFactory(); }
        }

        public IEventSerializer Serializer { get; set; }

        public IStoredEventRepository Repository { get; set; }

        public INotificationProducer NotificationProducer { get; set; }

        public void Append(DomainEvent domainEvent)
        {
            AppendEventStore(domainEvent, new EventStreamId(domainEvent.Id.ToString(), 0), 0);
        }

        public void Append(EventStreamId eventStreamId, IList<IDomainEvent> domainEvents)
        {
            var index = 0;
            foreach (var domainEvent in domainEvents)
            {
                AppendEventStore(domainEvent, eventStreamId, index++);
            }
        }

        public IEnumerable<StoredEvent> GetAllStoredEventsBetween(long lowStoredEventId, long highStoredEventId)
        {
            return Repository.AllStoredEventsBetween(lowStoredEventId, highStoredEventId);
        }

        public EventStream GetEventStreamSince(EventStreamId eventStreamId)
        {
            var storedEvents = Session.QueryOver<StoredEvent>()
                .Where(e => e.StreamName == eventStreamId.StreamName)
                .And(e => e.StreamVersion >= eventStreamId.StreamVersion)
                .OrderBy(e => e.StreamVersion).Asc.Future();

            var eventStream = CreateEventStream(storedEvents);

            return eventStream;
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

        private EventStream CreateEventStream(IEnumerable<StoredEvent> storedEvents)
        {
            var domainEvents = new List<DomainEvent>();
            long version = 0;
            foreach (var each in storedEvents)
            {
                domainEvents.Add(ToDomainEvent(each));
                version = each.StreamVersion;
            }

            return new EventStream(domainEvents, version);
        }

        protected void AppendEventStore(IDomainEvent domainEvent, EventStreamId startingEventStreamId, int index)
        {
            var storedEvent = ToStoredEvent(domainEvent, startingEventStreamId.StreamName,
                startingEventStreamId.StreamVersion + index);
            Repository.Append(storedEvent);

            NotificationProducer.Produce(this, storedEvent);
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