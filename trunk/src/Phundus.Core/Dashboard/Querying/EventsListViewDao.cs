namespace Phundus.Core.Dashboard.Querying
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using IdentityAndAccess.Users.Model;
    using Records;

    public class EventsQueries : ReadModelBase, IEventsQueries
    {
        public IEnumerable<EventsListViewRecord> FindAll()
        {
            return Session.QueryOver<EventsListViewRecord>().OrderBy(p => p.OccuredOnUtc).Desc.Take(20).List();
        }
    }

    public class EventsListViewDao : NHibernateReadModelBase<EventsListViewRecord>, IDomainEventHandler
    {
        public void Handle(DomainEvent domainEvent)
        {
            Process((dynamic) domainEvent);
        }

        public void Process(DomainEvent domainEvent)
        {
            var record = CreateRecord(domainEvent);
            record.Text = "Unformatiertes Ereignis: " + domainEvent.GetType().Name;
            Insert(record);
        }

        public void Process(UserLoggedIn domainEvent)
        {
            var record = CreateRecord(domainEvent);
            record.Text = "Benutzer hat sich eingeloggt: " + domainEvent.UserId;
            Insert(record);
        }

        public void Process(UserRegistered domainEvent)
        {
            var record = CreateRecord(domainEvent);
            record.Text = "Benutzer hat sich registriert: " + domainEvent.EmailAddress;
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