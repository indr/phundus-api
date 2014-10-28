namespace Phundus.Common.Messaging
{
    public interface INotificationConsumerFactory
    {
        INotificationConsumer[] GetNotificationConsumers();
    }
}