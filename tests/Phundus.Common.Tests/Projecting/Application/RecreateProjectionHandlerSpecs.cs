namespace Phundus.Common.Tests.Projecting.Application
{
    using System;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Projecting;
    using Common.Projecting.Application;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    [Subject(typeof (RecreateProjectionHandler))]
    public class RecreateProjectionHandlerSpecs : Observes<RecreateProjectionHandler>
    {
        protected static IProcessedNotificationTrackerStore trackerStore;
        protected static IProjectionFactory projectionFactory;
        protected static string projectionTypeName = "Phundus.Common.Tests.Projecting.NonExistingProjection";

        private Establish ctx = () =>
        {
            trackerStore = depends.on<IProcessedNotificationTrackerStore>();
            projectionFactory = depends.on<IProjectionFactory>();
        };


        public class when_projection_does_not_exist
        {
            private Establish ctx = () =>
                projectionFactory.setup(x => x.FindProjection(projectionTypeName)).Return((IProjection) null);

            private Because of = () =>
                sut.Handle(new RecreateProjection(new InitiatorId(), projectionTypeName));

            private It should_delete_tracker = () =>
                trackerStore.received(x => x.DeleteTracker(projectionTypeName));
        }

        public class when_projection_exists
        {
            private static IProjection projection;

            private Establish ctx = () =>
            {
                projection = fake.an<IProjection>();
                //projection.setup(x => x.GetEntityType()).Return(typeof (Object));
                projectionFactory.setup(x => x.FindProjection(projectionTypeName)).Return(projection);
            };

            private Because of = () =>
                spec.catch_exception(() =>
                    sut.Handle(new RecreateProjection(new InitiatorId(), projectionTypeName)));

            private It should_throw_not_implemented_exception = () =>
                spec.exception_thrown.ShouldBeAn<NotImplementedException>();

            //private It should_reset_tracker = () =>
            //    trackerStore.received(x => x.ResetTracker(projectionTypeName));
        }
    }
}