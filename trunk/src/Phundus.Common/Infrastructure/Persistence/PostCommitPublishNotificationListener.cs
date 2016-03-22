namespace Phundus.Common.Infrastructure.Persistence
{
    using Eventing;
    using NHibernate.Event;
    using Notifications;

    public class PostCommitPublishNotificationListener : IPostInsertEventListener
    {
        public void OnPostInsert(PostInsertEvent @event)
        {
            if (@event.Entity.GetType() != typeof (StoredEvent))
                return;

            NotificationPublisher.PublishNotification((StoredEvent)@event.Entity);
        }
    }
}