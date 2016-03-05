namespace Phundus.Common.Tests.Projecting
{
    using System;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Projecting;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Fakes;
    using Machine.Specifications;

    public class projection_processor_concern : Observes<NotificationToConsumersDispatcher>
    {
        protected static IEventConsumerFactory consumerFactory;
        protected static IStoredEventsProcessor projectionUpdater;
        protected static IProcessedNotificationTrackerStore trackerStore;

        protected static IEventConsumer consumer1;
        protected static IEventConsumer consumer2;

        private Establish ctx = () =>
        {
            consumer1 = fake.an<IEventConsumer>();
            consumer2 = fake.an<IEventConsumer>();

            consumerFactory = depends.on<IEventConsumerFactory>();
            consumerFactory.setup(x => x.GetConsumers()).Return(new[] {consumer1, consumer2});

            projectionUpdater = depends.on<IStoredEventsProcessor>();
            trackerStore = depends.on<IProcessedNotificationTrackerStore>();
        };
    }

    [Subject(typeof (NotificationToConsumersDispatcher))]
    public class when_updating : projection_processor_concern
    {
        private Establish ctx = () =>
            consumerFactory.setup(x => x.FindConsumer("typeName")).Return(consumer1);

        private Because of = () =>
            sut.Update("typeName");

        private It should_tell_projection_update = () =>
            projectionUpdater.received(x => x.Process(consumer1));
    }

    [Subject(typeof (NotificationToConsumersDispatcher))]
    public class when_projection_updater_updates_returns_true_as_not_done : projection_processor_concern
    {
        private static int calls;

        private Establish ctx = () =>
        {
            consumerFactory.setup(x => x.FindConsumer("typeName")).Return(consumer1);
            projectionUpdater.WhenToldTo(x => x.Process(consumer1)).Return(() => ++calls < 2);
        };

        private Because of = () =>
            sut.Update("typeName");

        private It should_call_update_twice = () =>
            projectionUpdater.received(x => x.Process(consumer1)).Twice();
    }

    [Subject(typeof (NotificationToConsumersDispatcher))]
    public class when_projection_update_throws_exception : projection_processor_concern
    {
        private static Exception exception;

        private Establish ctx = () =>
        {
            exception = new Exception("Error message");
            consumerFactory.setup(x => x.FindConsumer(consumer1.GetType().FullName)).Return(consumer1);
            projectionUpdater.setup(x => x.Process(consumer1)).Throw(exception);
        };

        private Because of = () =>
            sut.Update(consumer1.GetType().FullName);

        private It should_track_exception = () =>
            trackerStore.received(x => x.TrackException(consumer1.GetType().FullName, exception));
    }

    [Subject(typeof (NotificationToConsumersDispatcher))]
    public class when_handling_notification : projection_processor_concern
    {
        private Because of = () =>
            sut.Process(new Notification(1234, new DomainEvent(), 1));

        private It should_tell_projection_update = () =>
        {
            projectionUpdater.received(x => x.Process(consumer1));
            projectionUpdater.received(x => x.Process(consumer2));
        };
    }


    [Subject(typeof (NotificationToConsumersDispatcher))]
    public class when_projection_processor_processes_missed_notification_ : projection_processor_concern
    {
        private Because of = () =>
            sut.ProcessMissedNotifications();

        private It should_tell_projection_update = () =>
        {
            projectionUpdater.received(x => x.Process(consumer1));
            projectionUpdater.received(x => x.Process(consumer2));
        };
    }
}