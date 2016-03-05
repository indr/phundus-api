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
    using Rhino.Mocks;

    public class projection_processor_concern : Observes<ProjectionProcessor>
    {
        protected static IProjectionFactory projectionFactory;
        protected static IProjectionUpdater projectionUpdater;
        protected static IProcessedNotificationTrackerStore trackerStore;

        protected static IConsumer projection1;
        protected static IConsumer projection2;

        private Establish ctx = () =>
        {
            projection1 = fake.an<IConsumer>();
            projection2 = fake.an<IConsumer>();

            projectionFactory = depends.on<IProjectionFactory>();
            projectionFactory.setup(x => x.GetConsumers()).Return(new[] {projection1, projection2});

            projectionUpdater = depends.on<IProjectionUpdater>();            
            trackerStore = depends.on<IProcessedNotificationTrackerStore>();
        };
    }

    [Subject(typeof (ProjectionProcessor))]
    public class when_updating : projection_processor_concern
    {
        private Establish ctx = () =>
            projectionFactory.setup(x => x.FindConsumer("typeName")).Return(projection1);

        private Because of = () =>
            sut.Update("typeName");

        private It should_tell_projection_update = () =>
            projectionUpdater.received(x => x.Update(projection1));
    }

    [Subject(typeof (ProjectionProcessor))]
    public class when_projection_updater_updates_returns_true_as_not_done : projection_processor_concern
    {
        private static int calls;

        private Establish ctx = () =>
        {
            projectionFactory.setup(x => x.FindConsumer("typeName")).Return(projection1);
            projectionUpdater.WhenToldTo(x => x.Update(projection1)).Return(() => ++calls < 2);
        };

        private Because of = () =>
            sut.Update("typeName");

        private It should_call_update_twice = () =>
            projectionUpdater.received(x => x.Update(projection1)).Twice();
    }

    [Subject(typeof (ProjectionProcessor))]
    public class when_projection_update_throws_exception : projection_processor_concern
    {
        private static Exception exception;

        private Establish ctx = () =>
        {
            exception = new Exception("Error message");
            projectionFactory.setup(x => x.FindConsumer(projection1.GetType().FullName)).Return(projection1);
            projectionUpdater.setup(x => x.Update(projection1)).Throw(exception);
        };

        private Because of = () =>
            sut.Update(projection1.GetType().FullName);

        private It should_track_exception = () =>
            trackerStore.received(x => x.TrackException(projection1.GetType().FullName, exception));
    }

    [Subject(typeof (ProjectionProcessor))]
    public class when_handling_notification : projection_processor_concern
    {
        private Because of = () =>
            sut.Handle(new Notification(1234, new DomainEvent(), 1));

        private It should_tell_projection_update = () =>
        {
            projectionUpdater.received(x => x.Update(projection1));
            projectionUpdater.received(x => x.Update(projection2));
        };
    }


    [Subject(typeof (ProjectionProcessor))]
    public class when_projection_processor_processes_missed_notification_ : projection_processor_concern
    {
        private Because of = () =>
            sut.ProcessMissedNotifications();

        private It should_tell_projection_update = () =>
        {
            projectionUpdater.received(x => x.Update(projection1));
            projectionUpdater.received(x => x.Update(projection2));
        };
    }
}