namespace Phundus.Dashboard.Projections
{
    using System;
    using System.IO;
    using Common.Domain.Model;
    using Common.Eventing;
    using Common.Projecting;
    using Common.Projections;
    using IdentityAccess.Organizations.Model;
    using IdentityAccess.Users.Model;
    using Newtonsoft.Json;

    public class EventLogProjection : ProjectionBase<EventLogData>,
        ISubscribeTo<DomainEvent>,
        ISubscribeTo<UserLoggedIn>,
        ISubscribeTo<UserSignedUp>,
        ISubscribeTo<MembershipApplicationFiled>,
        ISubscribeTo<MembershipApplicationApproved>,
        ISubscribeTo<MembershipApplicationRejected>,
        ISubscribeTo<MemberLocked>,
        ISubscribeTo<MemberUnlocked>
    {
        public void Handle(DomainEvent domainEvent)
        {
            var record = CreateRow(domainEvent);
            record.Text = "Unformatiertes Ereignis: " + domainEvent.GetType().Name;
            Insert(record);
        }

        public void Handle(MemberLocked domainEvent)
        {
            var record = CreateRow(domainEvent);
            record.Text = string.Format("Benutzer {2} hat Mitglied {0} bei Organization {1} gesperrt.",
                domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }

        public void Handle(MembershipApplicationApproved domainEvent)
        {
            var record = CreateRow(domainEvent);
            record.Text =
                string.Format("Benutzer {2} hat Mitgliedschaft für Benutzer {0} bei Organization {1} bestätigt.",
                    domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }

        public void Handle(MembershipApplicationFiled domainEvent)
        {
            var record = CreateRow(domainEvent);
            record.Text =
                string.Format("Benutzer {2} hat Mitgliedschaft für Benutzer {0} bei Organization {1} beantragt.",
                    domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }

        public void Handle(MembershipApplicationRejected domainEvent)
        {
            var record = CreateRow(domainEvent);
            record.Text =
                string.Format("Benutzer {2} hat Mitgliedschaft für Benutzer {0} bei Organization {1} abgelehnt.",
                    domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }

        public void Handle(MemberUnlocked domainEvent)
        {
            var record = CreateRow(domainEvent);
            record.Text = string.Format("Benutzer {2} hat Mitglied {0} bei Organization {1} entsperrt.",
                domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.InitiatorId);
            Insert(record);
        }

        public void Handle(UserLoggedIn e)
        {
            var record = CreateRow(e);
            record.Text = "Benutzer hat sich eingeloggt: " + e.UserId;
            Insert(record);
        }

        public void Handle(UserSignedUp domainEvent)
        {
            var record = CreateRow(domainEvent);
            record.Text = "Benutzer hat sich registriert: " + domainEvent.EmailAddress;
            Insert(record);
        }

        private EventLogData CreateRow(DomainEvent @event)
        {
            var row = new EventLogData();
            row.EventGuid = @event.EventGuid;
            row.Name = @event.GetType().Name;
            row.OccuredOnUtc = @event.OccuredOnUtc;
            return row;
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