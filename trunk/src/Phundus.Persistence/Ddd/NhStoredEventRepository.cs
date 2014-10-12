namespace Phundus.Persistence.Ddd
{
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Transactions;
    using Common.Events;
    using Core.Ddd;

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
    }
}