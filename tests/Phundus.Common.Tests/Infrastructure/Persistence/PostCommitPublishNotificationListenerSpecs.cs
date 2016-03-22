namespace Phundus.Common.Tests.Infrastructure.Persistence
{
    using System;
    using Common.Eventing;
    using Common.Infrastructure.Persistence;
    using Common.Infrastructure.Persistence.Installers;
    using Common.Notifications;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using NHibernate.Event;
    using Rhino.Mocks;
    using Rhino.Mocks.Constraints;

    public class post_commit_publish_notification_listener_concern : Observes<PostCommitPublishNotificationListener>
    {
        protected static INotificationPublisher notificationPubliser;

        private Cleanup cleanup = () =>
            NotificationPublisher.Factory(null);

        private Establish ctx = () =>
        {
            notificationPubliser = fake.an<INotificationPublisher>();
            NotificationPublisher.Factory(() => notificationPubliser);

            storedEvent = new StoredEvent(Guid.NewGuid(), DateTime.UtcNow, "typeName", new byte[0], Guid.NewGuid(), 1);
        };

        protected static StoredEvent storedEvent;

        protected static PostInsertEvent makePostInsertEvent(object entity)
        {
            return new PostInsertEvent(storedEvent, null, null, null, null);
        }
    }

    [Subject(typeof (PostCommitPublishNotificationListener))]
    public class when_handling_stored_event : post_commit_publish_notification_listener_concern
    {
        private Because of = () =>
            sut.OnPostInsert(makePostInsertEvent(storedEvent));

        private It should_public_notification = () =>
            notificationPubliser.received(x => x.PublishNotification(storedEvent));
    }

    [Subject(typeof (PostCommitPublishNotificationListener))]
    public class when_handling_an_other_entity_type_than_stored_event :
        post_commit_publish_notification_listener_concern
    {
        private Because of = () =>
            sut.OnPostInsert(makePostInsertEvent(new Object()));

        private It should_not_publish_notification = () =>
            notificationPubliser.never_received(x => x.PublishNotification(Arg<StoredEvent>.Is.Anything));
    }
}