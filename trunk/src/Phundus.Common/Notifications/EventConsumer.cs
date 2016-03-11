namespace Phundus.Common.Notifications
{
    public interface IEventConsumer
    {
    }

    public interface IConsumes<in TDomainEvent> : IEventConsumer
    {
        void Handle(TDomainEvent e);
    }
}