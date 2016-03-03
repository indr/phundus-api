namespace Phundus.Common.Tests.Projecting
{
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Projecting;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public class projection_processor_concern : Observes<ProjectionProcessor>
    {
        protected static IProjectionFactory projectionFactory;
        protected static IProjectionUpdater projectionUpdater;

        protected static IProjection projection1;
        protected static IProjection projection2;

        private Establish ctx = () =>
        {
            projection1 = fake.an<IProjection>();
            projection2 = fake.an<IProjection>();

            projectionFactory = depends.on<IProjectionFactory>();
            projectionFactory.setup(x => x.GetProjections()).Return(new[] {projection1, projection2});

            projectionUpdater = depends.on<IProjectionUpdater>();
        };
    }
    
    [Subject(typeof (ProjectionProcessor))]
    public class when_updating : projection_processor_concern
    {
        private Establish ctx = () =>
            projectionFactory.setup(x => x.FindProjection("typeName")).Return(projection1);

        private Because of = () =>
            sut.Update("typeName");

        private It should_tell_projection_update = () =>
            projectionUpdater.received(x => x.Update(projection1));
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