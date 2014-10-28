namespace Phundus.Core.Dashboard.Port.Adapter.Persistence.View
{
    using System;
    using Application.Data;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Port.Adapter.Persistence;
    using IdentityAndAccess.Users.Model;

    public class NHibernateActivityProjection : NHibernateProjectionBase<ActivityData>, IDomainEventHandler
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
            record.Text = "Benutzer hat sich eingeloggt: " + domainEvent.EmailAddress;
            Insert(record);
        }

        public void Process(UserRegistered domainEvent)
        {
            var record = CreateRecord(domainEvent);
            record.Text = String.Format("Benutzer {0} hat sich angemeldet.", domainEvent.EmailAddress);
            Insert(record);
        }

        private static ActivityData CreateRecord(DomainEvent @event)
        {
            var record = new ActivityData();
            record.EventGuid = @event.Id;
            record.Name = @event.GetType().Name;
            record.OccuredOnUtc = @event.OccuredOnUtc;
            return record;
        }
    }
}