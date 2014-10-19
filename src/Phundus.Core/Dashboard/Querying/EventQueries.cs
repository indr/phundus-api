namespace Phundus.Core.Dashboard.Querying
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Events;
    using Common.Notifications;
    using Cqrs;
    using IdentityAndAccess.Users.Model;
    using Records;

    public interface IEventQueries
    {
        IEnumerable<EventsListViewRecord> FindAll();
    }

    public class EventsListViewDao : NHibernateReadModelBase<EventsListViewRecord>, IEventQueries,
        INotificationListener //, ISubscribeTo<UserRegistered>, ISubscribeTo<UserLoggedIn>
    {
        public IEnumerable<EventsListViewRecord> FindAll()
        {
            return Query().OrderBy(p => p.OccuredOnUtc).Desc.Take(20).List();
        }

        public void Handle(StoredEvent storedEvent, DomainEvent domainEvent)
        {
            Handle((dynamic) domainEvent);
        }

        public void Handle(DomainEvent domainEvent)
        {
            // Fallback
        }

        public void Handle(UserLoggedIn @event)
        {
            var record = CreateRecord(@event);
            Insert(record);
        }

        public void Handle(UserRegistered @event)
        {
            var record = CreateRecord(@event);
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