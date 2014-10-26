namespace Phundus.Core.Tests
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Core.Ddd;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public abstract class aggregate_root_concern<TAggregateRoot>
    {
        private static IWindsorContainer container;

        protected static IEventPublisher publisher;

        protected static Mock mock;

        protected Establish event_publisher = () =>
        {
            sut = default(TAggregateRoot);

            mock = new Mock();
            
            publisher = mock.stub<IEventPublisher>();

            container = new WindsorContainer();
            container.Register(Component.For<IEventPublisher>().Instance(publisher));
            EventPublisher.Container = container;
        };

        protected static TAggregateRoot sut { get; set; }
    }

    public abstract class concern<TClass> : Observes<TClass> where TClass : class
    {
        // ReSharper disable StaticFieldInGenericType

        private static IWindsorContainer container;

        protected static Mock mock = new Mock();

        protected static IEventPublisher publisher;

        // ReSharper restore StaticFieldInGenericType

        protected Establish event_publisher = () =>
        {
            publisher = depends.@on<IEventPublisher>();

            container = new WindsorContainer();
            container.Register(Component.For<IEventPublisher>().Instance(publisher));
            EventPublisher.Container = container;
        };
    }
}