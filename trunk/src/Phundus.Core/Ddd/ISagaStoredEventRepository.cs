namespace Phundus.Core.Ddd
{
    using System.Collections.Generic;
    using Common.Events;

    public interface ISagaStoredEventRepository
    {
        void Append(SagaStoredEvent sagaStoredEvent);
        IEnumerable<SagaStoredEvent> AllStoredEventsBetween(long lowStoredEventId, long highStoredEventId);
    }
}