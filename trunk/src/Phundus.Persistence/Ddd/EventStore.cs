namespace Phundus.Persistence.Ddd
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Events;
    using Core.Ddd;

    public class EventStore : IEventStore
    {
        private IEventSerializer _serializer = new EventSerializer();

        public IEventSerializer Serializer
        {
            get { return _serializer; }
            set { _serializer = value; }
        }

        public IStoredEventRepository Repository { get; set; }

        public void Append(DomainEvent domainEvent)
        {
            var serialization = Serializer.Serialize(domainEvent);

            var storedEvent = new StoredEvent(domainEvent.Id, domainEvent.OccuredOnUtc,
                domainEvent.GetType().Name, serialization);

            Repository.Append(storedEvent);
        }

        public IEnumerable<StoredEvent> AllStoredEventsBetween(long lowStoredEventId, long highStoredEventId)
        {
            return Repository.AllStoredEventsBetween(lowStoredEventId, highStoredEventId);
        }

        public long CountStoredEvents()
        {
            throw new System.NotImplementedException();
        }
    }
}