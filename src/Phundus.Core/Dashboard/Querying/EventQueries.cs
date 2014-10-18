namespace Phundus.Core.Dashboard.Querying
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Users.Model;
    using Records;

    public interface IEventQueries
    {
        IEnumerable<EventsListViewRecord> FindAll();
    }

    public class EventsListViewDao : NHibernateReadModelBase<EventsListViewRecord>, IEventQueries,
        ISubscribeTo<UserRegistered>, ISubscribeTo<UserLoggedIn>
    {
        public IEnumerable<EventsListViewRecord> FindAll()
        {
            return Query().OrderBy(p => p.OccuredOnUtc).Desc.Take(20).List();
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