namespace Phundus.Common.Notifications
{
    using Domain.Model;

    public interface IStoredEventsConsumer
    {
        void Handle(DomainEvent domainEvent);
    }
}