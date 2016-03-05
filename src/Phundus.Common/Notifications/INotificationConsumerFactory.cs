namespace Phundus.Common.Notifications
{
    public interface INotificationConsumerFactory
    {
        INotificationHandler[] GetNotificationConsumers();
    }
}