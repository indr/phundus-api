namespace Phundus.Common.Eventing
{
    using System.Collections.Generic;
    using Domain.Model;

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
}