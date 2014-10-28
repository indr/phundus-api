namespace Phundus.Common.Messaging
{
    using Notifications;

    public interface INotificationConsumer
    {
        void Consume(Notification notification);
    }
}