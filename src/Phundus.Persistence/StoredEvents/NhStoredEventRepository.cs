namespace Phundus.Persistence.StoredEvents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Eventing;
    using Ddd;

    public class NhStoredEventRepository : NhRepositoryBase<StoredEvent>, IStoredEventRepository
    {
        public void Append(StoredEvent storedEvent)
        {
            Session.Save(storedEvent);
        }

        public IEnumerable<StoredEvent> AllStoredEventsBetween(long lowStoredEventId, long highStoredEventId)
        {
            var query = from se in Entities
                where se.EventId >= lowStoredEventId
                      && se.EventId <= highStoredEventId
                select se;

            return query.ToList();
        }

        public long CountStoredEvents()
        {
            return (from se in Entities select se).Count();
        }

        public long GetMaxNotificationId()
        {
            var result = (from se in Entities select se.EventId).Max(x => (long?) x);
            if (result.HasValue)
                return result.Value;
            return 0;
        }

        public IEnumerable<StoredEvent> GetStoredEvents(Guid aggregateId)
        {
            return Session.QueryOver<StoredEvent>()
                .Where(p => p.AggregateId == aggregateId)
                .OrderBy(p => p.Version).Asc
                .ThenBy(p => p.EventId).Asc
                .Future();
        }
    }
}