namespace Phundus.Common.Tests.Notifications
{
    using System.Collections.Generic;
    using Common.Eventing;
    using Common.Notifications;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Rhino.Mocks;

    public class notification_log_factory_concern : Observes<NotificationLogFactory>
    {
        protected static IEventStore eventStore;
        protected static NotificationLog log;

        private Establish ctx = () =>
        {
            log = null;
            eventStore = depends.on<IEventStore>();
            eventStore.setup(x => x.AllStoredEventsBetween(Arg<long>.Is.Anything, Arg<long>.Is.Anything))
                .Return(new List<StoredEvent>());
        };
    }

    public class get_current_notification_log_when_event_store_is_empty : notification_log_factory_concern
    {
        private Establish ctx = () =>
            eventStore.setup(x => x.GetMaxNotificationId()).Return(0);

        private Because of = () =>
            log = sut.CreateCurrentNotificationLog();

        private It should_have_log_id = () =>
            log.NotificationLogId.ShouldEqual("1,20");

        private It should_have_no_previous = () =>
            log.PreviousNotificationLogId.ShouldBeNull();

        private It should_not_be_archived = () =>
            log.IsArchived.ShouldBeFalse();
    }

    public class get_current_notification_log_when_event_store_is_not_empty : notification_log_factory_concern
    {
        private Establish ctx = () =>
            eventStore.setup(x => x.GetMaxNotificationId()).Return(1011);

        private Because of = () =>
            log = sut.CreateCurrentNotificationLog();

        private It should_have_log_id = () =>
            log.NotificationLogId.ShouldEqual("1001,1020");

        private It should_have_previous = () =>
            log.PreviousNotificationLogId.ShouldEqual("981,1000");

        private It should_not_be_archived = () =>
            log.IsArchived.ShouldBeFalse();
    }

    public class get_notification : notification_log_factory_concern
    {
        private Because of = () =>
            log = sut.CreateNotificationLog("1001,1020");

        private It should_have_log_id = () =>
            log.NotificationLogId.ShouldEqual("1001,1020");

        private It should_not_be_archived = () =>
            log.IsArchived.ShouldBeFalse();
    }
}