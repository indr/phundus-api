namespace Phundus.Core.Dashboard.Port.Adapter.Persistence.View
{
    using System;
    using Application.Data;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Port.Adapter.Persistence;
    using IdentityAndAccess.Users.Model;
    using Inventory.Domain.Model.Management;

    public class NHibernateActivityProjection : NHibernateProjectionBase<ActivityData>, IDomainEventHandler
    {
        public void Handle(IDomainEvent domainEvent)
        {
            Process((dynamic) domainEvent);
        }

        public void Process(DomainEvent domainEvent)
        {
            var record = CreateRecord(domainEvent);
            record.Text = "Unformatiertes Ereignis: " + domainEvent.GetType().Name;
            Save(record);
        }

        public void Process(UserLoggedIn domainEvent)
        {
            var record = CreateRecord(domainEvent);
            record.Text = "Benutzer hat sich eingeloggt: " + domainEvent.EmailAddress;
            Save(record);
        }

        public void Process(UserRegistered domainEvent)
        {
            var record = CreateRecord(domainEvent);
            record.Text = String.Format("Benutzer {0} hat sich angemeldet.", domainEvent.EmailAddress);
            Save(record);
        }

        public void Process(QuantityAvailableChanged e)
        {
            var record = CreateRecord(e);
            record.Text = String.Format("Verfügbarkeit des Artikels {4} von {0} bis {1} hat sich um {2} geändert.",
                new object[] {e.FromUtc.ToString("G"), e.ToUtc.ToString("G"), e.Change, e.ArticleId});
            Save(record);
        }

        private static ActivityData CreateRecord(DomainEvent domainEvent)
        {
            var record = new ActivityData();
            record.EventGuid = domainEvent.Id;
            record.Name = domainEvent.GetType().Name;
            record.OccuredOnUtc = domainEvent.OccuredOnUtc;
            return record;
        }
    }
}