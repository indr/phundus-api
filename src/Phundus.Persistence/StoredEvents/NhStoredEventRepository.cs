namespace Phundus.Persistence.StoredEvents
{
    using System.Collections.Generic;
    using System.Linq;
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

        public long CountStoredEvents()
        {
            return (from se in Entities select se).Count();
        }

        public long GetMaxNotificationId()
        {
            return (from se in Entities select se.EventId).Max();
        }
    }
}