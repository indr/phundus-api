namespace Phundus.Persistence.Ddd
{
    using Core.Ddd;

    public class EventStore : IEventStore, ISubscribeTo<DomainEvent>
    {
        public IEventSerializer Serializer { get; set; }

        public IStoredEventRepository Repository { get; set; }

        public void Append(DomainEvent domainEvent)
        {
            var serialization = Serializer.Serialize(domainEvent);

            var storedEvent = new StoredEvent(domainEvent.Id, domainEvent.OccuredOnUtc,
                domainEvent.GetType().Name, serialization);

            Repository.Add(storedEvent);
        }

        public void Handle(DomainEvent @event)
        {
            Append(@event);
        }
    }
}