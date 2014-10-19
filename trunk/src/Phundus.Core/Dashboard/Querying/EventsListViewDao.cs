namespace Phundus.Core.Dashboard.Querying
{
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using IdentityAndAccess.Users.Model;
    using Records;

    public class EventsListViewDao : NHibernateReadModelBase<EventsListViewRecord>, IEventQueries, IDomainEventHandler
    {
        public void Handle(DomainEvent domainEvent)
        {
            Process((dynamic) domainEvent);
        }

        public IEnumerable<EventsListViewRecord> FindAll()
        {
            return Query().OrderBy(p => p.OccuredOnUtc).Desc.Take(20).List();
        }

        public void Process(DomainEvent domainEvent)
        {
            // Fallback
        }

        public void Process(UserLoggedIn domainEvent)
        {
            var record = CreateRecord(domainEvent);
            Insert(record);
        }

        public void Process(UserRegistered domainEvent)
        {
            var record = CreateRecord(domainEvent);
            Insert(record);
        }

        private static EventsListViewRecord CreateRecord(DomainEvent @event)
        {
            var record = new EventsListViewRecord();
            record.EventGuid = @event.Id;
            record.Name = @event.GetType().Name;
            record.OccuredOnUtc = @event.OccuredOnUtc;
            return record;
        }
    }
}