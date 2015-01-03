namespace Phundus.Persistence.SagaStoredEvents
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Events;

    public interface ISagaEventStore
    {
        void Append(EventStreamId eventStreamId, ICollection<IDomainEvent> domainEvents);

        EventStream GetEventStreamSince(EventStreamId eventStreamId);

        DomainEvent ToDomainEvent(SagaStoredEvent sagaStoredEvent);
    }
}