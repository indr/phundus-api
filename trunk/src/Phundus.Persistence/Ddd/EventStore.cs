namespace Phundus.Persistence.Ddd
{
    using System;
    using Core.Ddd;
    using NHibernate;

    public class StoredEvent
    {
        public StoredEvent(Guid eventId, DateTime occuredOnUtc, string name, byte[] serialization)
        {
            EventId = eventId;
            OccuredOnUtc = occuredOnUtc;
            Name = name;
            Serialization = serialization;
        }

        public long Id { get; protected set; }
        public Guid EventId { get; protected set; }
        public Guid AggregateId { get; protected set; }
        public DateTime OccuredOnUtc { get; protected set; }
        public string Name { get; protected set; }
        public byte[] Serialization { get; protected set; }
    }

    public class EventStore : IEventStore
    {
        public IEventSerializer Serializer { get; set; }

        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get { return SessionFactory(); }
        }

        public void Append(DomainEvent domainEvent)
        {
            var serialization = Serializer.Serialize(domainEvent);

            var storedEvent = new StoredEvent(domainEvent.Id, domainEvent.OccuredOnUtc,
                domainEvent.GetType().Name, serialization);

            Session.Save(storedEvent);
        }
    }
}