namespace Phundus.Common.Events
{
    using System.Collections.Generic;
    using Domain.Model;

    public interface IEventStore
    {
        void Append(DomainEvent domainEvent);

        void Append(EventStreamId eventStreamId, IList<IDomainEvent> domainEvents);

        long CountStoredEvents();

        IEnumerable<StoredEvent> GetAllStoredEventsBetween(long lowStoredEventId, long highStoredEventId);

        EventStream GetEventStreamSince(EventStreamId eventStreamId);

        long GetMaxNotificationId();

        DomainEvent ToDomainEvent(StoredEvent storedEvent);
    }
}