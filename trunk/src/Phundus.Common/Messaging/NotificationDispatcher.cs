namespace Phundus.Common.Messaging
{
    using Notifications;

    public class NotificationDispatcher
    {
        private readonly INotificationConsumerFactory _factory;

        public NotificationDispatcher(INotificationConsumerFactory factory)
        {
            _factory = factory;
        }

        public void Dispatch(Notification notification)
        {
            var consumers = _factory.GetNotificationConsumers();

            foreach (var each in consumers)
            {
                each.Consume(notification);
            }
        }
    }
}