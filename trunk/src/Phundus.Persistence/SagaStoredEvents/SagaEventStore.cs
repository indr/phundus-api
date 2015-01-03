namespace Phundus.Persistence.SagaStoredEvents
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Events;
    using Core.Ddd;
    using NHibernate;

    public class SagaEventStore : ISagaEventStore
    {
        public Func<ISession> SessionFactory { get; set; }

        protected ISession Session
        {
            get { return SessionFactory(); }
        }

        public ISagaStoredEventRepository Repository { get; set; }

        public IEventSerializer Serializer { get; set; }

        public void Append(EventStreamId eventStreamId, ICollection<IDomainEvent> domainEvents)
        {
            var index = 0;
            foreach (var domainEvent in domainEvents)
            {
                AppendEventStore(domainEvent, eventStreamId, index++);
            }
        }

        private void AppendEventStore(IDomainEvent domainEvent, EventStreamId startingEventStreamId, int index)
        {
            var sagaStoredEvent = ToSagaStoredEvent(domainEvent, startingEventStreamId.StreamName,
                startingEventStreamId.StreamVersion + index);
            Repository.Append(sagaStoredEvent);
        }

        private SagaStoredEvent ToSagaStoredEvent(IDomainEvent domainEvent, string streamName, long streamVersion)
        {
            var serialization = Serializer.Serialize(domainEvent);

            var domainEventType = domainEvent.GetType();
            var typeName = domainEventType.FullName + ", " + domainEventType.Assembly.GetName().Name;

            var sagaStoredEvent = new SagaStoredEvent(domainEvent.Id, domainEvent.OccuredOnUtc,
                typeName, serialization, streamName, streamVersion);

            return sagaStoredEvent;
        }

        public EventStream GetEventStreamSince(EventStreamId eventStreamId)
        {
            var storedEvents = Session.QueryOver<SagaStoredEvent>()
                .Where(e => e.StreamName == eventStreamId.StreamName)
                .And(e => e.StreamVersion >= eventStreamId.StreamVersion)
                .OrderBy(e => e.StreamVersion).Asc.Future();

            return CreateEventStream(storedEvents);
        }

        private EventStream CreateEventStream(IEnumerable<SagaStoredEvent> storedEvents)
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

        public DomainEvent ToDomainEvent(SagaStoredEvent sagaStoredEvent)
        {
            var domainEventType = Type.GetType(sagaStoredEvent.TypeName, true);

            return (DomainEvent)Serializer.Deserialize(domainEventType, sagaStoredEvent.EventGuid,
                sagaStoredEvent.OccuredOnUtc, sagaStoredEvent.Serialization);
        }
    }
}