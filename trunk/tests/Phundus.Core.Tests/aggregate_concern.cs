namespace Phundus.Tests
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Machine.Specifications;
    using Phundus.Ddd;

    public abstract class aggregate_concern<TAggregate>
    {
        // ReSharper disable StaticFieldInGenericType

        private static IWindsorContainer container;

        protected static Mock mock = new Mock();

        protected static IEventPublisher publisher;

        protected static TAggregate sut;

        // ReSharper restore StaticFieldInGenericType

        protected Establish event_publisher = () =>
        {
            publisher = mock.mock<IEventPublisher>();

            container = new WindsorContainer();
            container.Register(Component.For<IEventPublisher>().Instance(publisher));
            EventPublisher.Container = container;
        };
    }
}