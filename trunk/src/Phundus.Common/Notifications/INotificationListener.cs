namespace Phundus.Common.Notifications
{
    using Domain.Model;
    using Events;

    public interface INotificationListener
    {
        void Handle(StoredEvent storedEvent, DomainEvent domainEvent);
    }
}