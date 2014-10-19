namespace Phundus.Core.Dashboard.Querying
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using IdentityAndAccess.Users.Model;
    using Records;

    public interface IEventsQueries
    {
        IEnumerable<EventsRecord> FindAll();
    }

    public class EventsReadModel : NHibernateReadModelBase<EventsRecord>, IEventsQueries, IDomainEventHandler
    {
        public void Handle(DomainEvent domainEvent)
        {
            Process((dynamic) domainEvent);
        }

        public IEnumerable<EventsRecord> FindAll()
        {
            return Query().OrderBy(p => p.OccuredOnUtc).Desc.Take(20).List();
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

        private static EventsRecord CreateRecord(DomainEvent @event)
        {
            var record = new EventsRecord();
            record.EventGuid = @event.Id;
            record.Name = @event.GetType().Name;
            record.OccuredOnUtc = @event.OccuredOnUtc;
            return record;
        }
    }
}