namespace Phundus.Core.Tests.Inventory
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Core.Cqrs;
    using Core.Inventory.Commands;
    using Core.Inventory.Repositories;
    using Ddd;
    using developwithpassion.specifications.rhinomocks;
    using IdentityAndAccess.Queries;
    using Machine.Specifications;

    public abstract class handler_concern<TCommand, THandler> : Observes<THandler> where THandler: class, IHandleCommand<TCommand>
    {
        private static IWindsorContainer container;

        protected static TCommand command;

        protected static IEventPublisher publisher;

        protected static IMemberInRole memberInRole;

        protected static IArticleRepository repository;

        protected Establish event_publisher = () =>
        {
            publisher = depends.on<IEventPublisher>();

            container = new WindsorContainer();
            container.Register(Component.For<IEventPublisher>().Instance(publisher));
            EventPublisher.Container = container;

            memberInRole = depends.on<IMemberInRole>();
            repository = depends.on<IArticleRepository>();
        };

        public Because of = () => sut.Handle(command);
    }
}