namespace Phundus.Common.Notifications
{
    using Domain.Model;

    public interface IStoredEventsConsumer
    {
        void Handle(DomainEvent e);
    }

    public interface IConsumes
    {
    }

    public interface IConsumes<in TDomainEvent> : IConsumes
    {
        void Handle(TDomainEvent e);
    }
}