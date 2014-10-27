namespace Phundus.Common.Events
{
    using System.Collections.Generic;
    using Domain.Model;

    public interface IEventStore
    {
        IEnumerable<StoredEvent> AllStoredEventsBetween(long lowStoredEventId, long highStoredEventId);

        void Append(DomainEvent domainEvent);
        void Append(EventStreamId eventStreamId, IList<IDomainEvent> domainEvents);

        long CountStoredEvents();
        long GetMaxNotificationId();
        DomainEvent ToDomainEvent(StoredEvent storedEvent);
    }
}