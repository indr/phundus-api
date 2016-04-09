namespace Phundus.Common.Tests.Projecting.Application
{
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Projecting;
    using Common.Projecting.Application;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public abstract class when_handling_reset_projection : Observes<ResetProjectionHandler>
    {
        protected static IProcessedNotificationTrackerStore trackerStore;
        protected static IProjectionFactory projectionFactory;
        protected static string projectionTypeName = "Phundus.Common.Tests.Projecting.NonExistingProjection";

        private Establish ctx = () =>
        {
            trackerStore = depends.on<IProcessedNotificationTrackerStore>();
            projectionFactory = depends.on<IProjectionFactory>();
        };

        private Because of = () =>
            sut.Handle(new ResetProjection(new InitiatorId(), projectionTypeName));
    }

    [Subject(typeof (ResetProjectionHandler))]
    public class when_projection_does_not_exist : when_handling_reset_projection
    {
        private Establish ctx = () =>
            projectionFactory.setup(x => x.FindProjection(projectionTypeName)).Return((IProjection) null);

        private It should_not_delete_tracker = () =>
            trackerStore.never_received(x => x.DeleteTracker(projectionTypeName));
    }

    [Subject(typeof (ResetProjectionHandler))]
    public class when_projection_exists_and_can_reset_is_true : when_handling_reset_projection
    {
        private static IProjection projection;

        private Establish ctx = () =>
        {
            projection = fake.an<IProjection>();
            projectionFactory.setup(x => x.FindProjection(projectionTypeName)).Return(projection);
            projection.setup(x => x.CanReset).Return(true);
        };

        private It should_reset_projection = () =>
            projection.received(x => x.Reset());

        private It should_reset_tracker = () =>
            trackerStore.received(x => x.ResetTracker(projectionTypeName));
    }

    [Subject(typeof (ResetProjectionHandler))]
    public class when_projection_exists_but_can_reset_is_false : when_handling_reset_projection
    {
        private static IProjection projection;

        private Establish ctx = () =>
        {
            projection = fake.an<IProjection>();
            projectionFactory.setup(x => x.FindProjection(projectionTypeName)).Return(projection);
            projection.setup(x => x.CanReset).Return(false);
        };

        private It should_not_reset_projection = () =>
            projection.never_received(x => x.Reset());

        private It should_not_reset_tracker = () =>
            trackerStore.never_received(x => x.ResetTracker(projectionTypeName));
    }
}