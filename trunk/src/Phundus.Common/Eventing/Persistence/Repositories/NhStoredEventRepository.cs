namespace Phundus.Persistence.Eventing.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Eventing;
    using Common.Infrastructure.Persistence;
    using NHibernate.Criterion;

    public class NhStoredEventRepository : NhRepositoryBase<StoredEvent>, IStoredEventRepository
    {
        public void Append(StoredEvent storedEvent)
        {
            Session.Save(storedEvent);
        }

        public void Append(IEnumerable<StoredEvent> storedEvents, Guid aggregateId, int? expectedVersion)
        {
            var version = QueryOver()
                .Where(p => p.AggregateId == aggregateId)
                .Select(Projections.Max("Version"))
                .SingleOrDefault<int?>() ?? 0;

            if (expectedVersion.HasValue)
            {
                if (version != expectedVersion)
                {
                    throw new EventStoreConcurrencyException(
                    version, expectedVersion.Value, aggregateId);
                }
            }

            foreach (var each in storedEvents)
            {
                Session.Save(each);
            }
        }

        public IEnumerable<StoredEvent> AllStoredEventsBetween(long lowStoredEventId, long highStoredEventId)
        {
            var query = from se in Entities
                where se.EventId >= lowStoredEventId
                      && se.EventId <= highStoredEventId
                      orderby se.EventId
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

    public class EventStoreConcurrencyException : Exception
    {
        public EventStoreConcurrencyException(int version, int expectedVersion, Guid aggregateId) : base(String.Format(
            @"Could not append to stream {2}, version {0}, expected version {1}.", version, expectedVersion, aggregateId))
        {
            
        }
    }
}