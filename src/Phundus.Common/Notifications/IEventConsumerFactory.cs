namespace Phundus.Common.Notifications
{
    public interface IEventConsumerFactory
    {
        IEventConsumer FindConsumer(string fullName);
        IEventConsumer[] GetConsumers();
    }
}