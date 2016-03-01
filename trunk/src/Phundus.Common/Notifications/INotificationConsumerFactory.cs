namespace Phundus.Common.Notifications
{
    public interface INotificationConsumerFactory
    {
        INotificationConsumer[] GetNotificationConsumers();
    }
}