namespace Phundus.Common.Notifications
{
    using Domain.Model;

    public interface IDomainEventHandler
    {
        void Handle(IDomainEvent domainEvent);
    }
}