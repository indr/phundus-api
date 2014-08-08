namespace Phundus.Core.Tests
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Core.Cqrs;
    using Ddd;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public abstract class handler_concern<TCommand, THandler> : Observes<THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        private static IWindsorContainer container;

        protected static TCommand command;

        protected static IEventPublisher publisher;

        protected Establish event_publisher = () =>
        {
            publisher = depends.on<IEventPublisher>();

            container = new WindsorContainer();
            container.Register(Component.For<IEventPublisher>().Instance(publisher));
            EventPublisher.Container = container;
        };

        public Because of = () => sut.Handle(command);
    }
}