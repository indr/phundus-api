namespace Phundus.Common.Notifications
{
    public interface IConsumer
    {
    }

    public interface IConsumes<in TDomainEvent> : IConsumer
    {
        void Consume(TDomainEvent e);
    }
}