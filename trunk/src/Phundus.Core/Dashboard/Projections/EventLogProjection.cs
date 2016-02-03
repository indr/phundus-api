namespace Phundus.Dashboard.Projections
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using IdentityAccess.Organizations.Model;
    using IdentityAccess.Users.Model;
    using Queries;

    public class EventLogProjection : NHibernateReadModelBase<EventLogProjectionRow>, IEventLogQueries,
        IDomainEventHandler
    {
        public void Handle(DomainEvent domainEvent)
        {
            Process((dynamic) domainEvent);
        }

        public IEnumerable<EventLogProjectionRow> FindMostRecent20()
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
            record.Text =
                string.Format("Benutzer {2} hat Mitgliedschaft für Benutzer {0} bei Organization {1} beantragt.",
                    domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }

        public void Process(MembershipApplicationApproved domainEvent)
        {
            var record = CreateRecord(domainEvent);
            record.Text =
                string.Format("Benutzer {2} hat Mitgliedschaft für Benutzer {0} bei Organization {1} bestätigt.",
                    domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }

        public void Process(MembershipApplicationRejected domainEvent)
        {
            var record = CreateRecord(domainEvent);
            record.Text =
                string.Format("Benutzer {2} hat Mitgliedschaft für Benutzer {0} bei Organization {1} abgelehnt.",
                    domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }

        public void Process(MemberLocked domainEvent)
        {
            var record = CreateRecord(domainEvent);
            record.Text = string.Format("Benutzer {2} hat Mitglied {0} bei Organization {1} gesperrt.",
                domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }

        public void Process(MemberUnlocked domainEvent)
        {
            var record = CreateRecord(domainEvent);
            record.Text = string.Format("Benutzer {2} hat Mitglied {0} bei Organization {1} entsperrt.",
                domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }

        private static EventLogProjectionRow CreateRecord(DomainEvent @event)
        {
            var record = new EventLogProjectionRow();
            record.EventGuid = @event.EventGuid;
            record.Name = @event.GetType().Name;
            record.OccuredOnUtc = @event.OccuredOnUtc;
            return record;
        }
    }
}