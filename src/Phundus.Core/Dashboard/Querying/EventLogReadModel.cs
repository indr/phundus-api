namespace Phundus.Dashboard.Querying
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using IdentityAccess.Organizations.Model;
    using IdentityAccess.Users.Model;
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
            return QueryOver().OrderBy(p => p.OccuredOnUtc).Desc.Take(20).List();
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

        public void Process(UserSignedUp domainEvent)
        {
            var record = CreateRecord(domainEvent);
            record.Text = "Benutzer hat sich registriert: " + domainEvent.EmailAddress;
            Insert(record);
        }

        public void Process(MembershipApplicationFiled domainEvent)
        {
            var record = CreateRecord(domainEvent);
            record.Text = string.Format("Benutzer {2} hat Mitgliedschaft für Benutzer {0} bei Organization {1} beantragt.",
                domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }

        public void Process(MembershipApplicationApproved domainEvent)
        {
            var record = CreateRecord(domainEvent);
            record.Text = string.Format("Benutzer {2} hat Mitgliedschaft für Benutzer {0} bei Organization {1} bestätigt.",
                domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }

        public void Process(MembershipApplicationRejected domainEvent)
        {
            var record = CreateRecord(domainEvent);
            record.Text = string.Format("Benutzer {2} hat Mitgliedschaft für Benutzer {0} bei Organization {1} abgelehnt.",
                domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }

        private static EventLogRecord CreateRecord(DomainEvent @event)
        {
            var record = new EventLogRecord();
            record.EventGuid = @event.EventGuid;
            record.Name = @event.GetType().Name;
            record.OccuredOnUtc = @event.OccuredOnUtc;
            return record;
        }
    }
}