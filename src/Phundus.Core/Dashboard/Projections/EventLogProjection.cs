namespace Phundus.Dashboard.Projections
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Projections;
    using Cqrs;
    using IdentityAccess.Organizations.Model;
    using IdentityAccess.Users.Model;
    using Newtonsoft.Json;

    public interface IEventLogQueries
    {
        IEnumerable<EventLogData> FindMostRecent20();
    }

    public class EventLogProjection : ReadModelBase<EventLogData>, IEventLogQueries,
        IStoredEventsConsumer
    {
        public IEnumerable<EventLogData> FindMostRecent20()
        {
            return QueryOver().OrderBy(p => p.OccuredOnUtc).Desc.Take(20).List();
        }

        public void Handle(DomainEvent e)
        {
            Process((dynamic) e);
        }

        private EventLogData CreateRow(DomainEvent @event)
        {
            var row = new EventLogData();
            row.EventGuid = @event.EventGuid;
            row.Name = @event.GetType().Name;
            row.OccuredOnUtc = @event.OccuredOnUtc;
            return row;
        }

        public void Process(DomainEvent domainEvent)
        {
            var record = CreateRow(domainEvent);
            record.Text = "Unformatiertes Ereignis: " + domainEvent.GetType().Name;
            Insert(record);
        }

        public void Process(UserLoggedIn domainEvent)
        {
            var record = CreateRow(domainEvent);
            record.Text = "Benutzer hat sich eingeloggt: " + domainEvent.UserId;
            Insert(record);
        }

        public void Process(UserSignedUp domainEvent)
        {
            var record = CreateRow(domainEvent);
            record.Text = "Benutzer hat sich registriert: " + domainEvent.EmailAddress;
            Insert(record);
        }

        public void Process(MembershipApplicationFiled domainEvent)
        {
            var record = CreateRow(domainEvent);
            record.Text =
                string.Format("Benutzer {2} hat Mitgliedschaft für Benutzer {0} bei Organization {1} beantragt.",
                    domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }

        public void Process(MembershipApplicationApproved domainEvent)
        {
            var record = CreateRow(domainEvent);
            record.Text =
                string.Format("Benutzer {2} hat Mitgliedschaft für Benutzer {0} bei Organization {1} bestätigt.",
                    domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }

        public void Process(MembershipApplicationRejected domainEvent)
        {
            var record = CreateRow(domainEvent);
            record.Text =
                string.Format("Benutzer {2} hat Mitgliedschaft für Benutzer {0} bei Organization {1} abgelehnt.",
                    domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }

        public void Process(MemberLocked domainEvent)
        {
            var record = CreateRow(domainEvent);
            record.Text = string.Format("Benutzer {2} hat Mitglied {0} bei Organization {1} gesperrt.",
                domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }

        public void Process(MemberUnlocked domainEvent)
        {
            var record = CreateRow(domainEvent);
            record.Text = string.Format("Benutzer {2} hat Mitglied {0} bei Organization {1} entsperrt.",
                domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }
    }

    public class EventLogData
    {
        [JsonProperty("eventGuid")]
        public virtual Guid EventGuid { get; set; }

        [JsonProperty("occuredOnUtc")]
        public virtual DateTime OccuredOnUtc { get; set; }

        [JsonProperty("type")]
        public virtual string Name { get; set; }

        [JsonProperty("data")]
        [JsonConverter(typeof (RawJsonConverter))]
        public virtual string JsonData { get; protected set; }

        public virtual string Text { get; set; }

        public virtual void SetData(object data)
        {
            var stringWriter = new StringWriter();
            var settings = new JsonSerializerSettings();
            JsonSerializer.Create(settings).Serialize(stringWriter, data);
            JsonData = stringWriter.GetStringBuilder().ToString();
        }
    }
}