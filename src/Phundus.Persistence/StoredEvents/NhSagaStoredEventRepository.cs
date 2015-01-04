namespace Phundus.Persistence.StoredEvents
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Events;
    using Core.Ddd;

    public class NhSagaStoredEventRepository : NhRepositoryBase<SagaStoredEvent>, ISagaStoredEventRepository
    {
        public void Append(SagaStoredEvent sagaStoredEvent)
        {
            Session.Save(sagaStoredEvent);
        }

        public IEnumerable<SagaStoredEvent> AllStoredEventsBetween(long lowStoredEventId, long highStoredEventId)
        {
            var query = from se in Entities
                where se.EventId >= lowStoredEventId
                      && se.EventId <= highStoredEventId
                select se;

            return query.ToList();
        }
    }
}