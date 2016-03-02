namespace Phundus.Common.Tests.Projecting.Application
{
    using Castle.MicroKernel;
    using Common.Domain.Model;
    using Common.Projecting;
    using Common.Projecting.Application;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Notifications;

    [Subject(typeof (ResetProjectionHandler))]
    public class when_handling_reset_projection : Observes<ResetProjectionHandler>
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

        public class when_projection_does_not_exist
        {
            private Establish ctx = () =>
                projectionFactory.setup(x => x.FindProjection(projectionTypeName)).Return((IProjection)null);

            private It should_delete_tracker = () =>
                trackerStore.received(x => x.DeleteTracker(projectionTypeName));
        }

        public class when_projection_exists
        {
            private static IProjection projection;

            private Establish ctx = () =>
            {
                projection = fake.an<IProjection>();
                projectionFactory.setup(x => x.FindProjection(projectionTypeName)).Return(projection);
            };

            private It should_reset_projection = () =>
                projection.received(x => x.Reset());

            private It should_reset_tracker = () =>
                trackerStore.received(x => x.ResetTracker(projectionTypeName));
        }
    }
}