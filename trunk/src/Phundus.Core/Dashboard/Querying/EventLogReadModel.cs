namespace Phundus.Core.Dashboard.Querying
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using IdentityAndAccess.Users.Model;
    using Records;

    public interface IEventLogQueries
    {
        IEnumerable<EventLogRecord> FindMostRecent20();
    }

    public class EventLogReadModel : NHibernateReadModelBase<EventLogRecord>, IEventLogQueries, IDomainEventHandler
    {
        public void Handle(DomainEvent domainEvent)
        {
            Process((dynamic) domainEvent);
        }

        public IEnumerable<EventLogRecord> FindMostRecent20()
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

        private static EventLogRecord CreateRecord(DomainEvent @event)
        {
            var record = new EventLogRecord();
            record.EventGuid = @event.Id;
            record.Name = @event.GetType().Name;
            record.OccuredOnUtc = @event.OccuredOnUtc;
            return record;
        }
    }
}