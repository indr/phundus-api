namespace Phundus.Common.Projecting
{
    using Notifications;

    public interface IEventConsumerFactory
    {
        IEventConsumer FindConsumer(string fullName);
        IEventConsumer[] GetConsumers();
    }
}