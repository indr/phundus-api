namespace Phundus.Core
{
    using System.Reflection;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common.Events;
    using Common.Notifications;
    using Cqrs;
    using Ddd;

    public class CoreInstaller : IWindsorInstaller
    {
        private readonly Assembly _assemblyContainingCommandsAndHandlers;

        public CoreInstaller() : this(typeof (CoreInstaller).Assembly)
        {
        }

        public CoreInstaller(Assembly assemblyContainingCommandsAndHandlers)
        {
            _assemblyContainingCommandsAndHandlers = assemblyContainingCommandsAndHandlers;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<TypedFactoryFacility>();

            container.Register(
                Component.For<ITypedFactoryComponentSelector>().ImplementedBy<CommandHandlerSelector>(),
                Component.For<AutoReleaseCommandHandlerInterceptor>(),
                Types.FromAssembly(_assemblyContainingCommandsAndHandlers)
                    .BasedOn(typeof (IHandleCommand<>))
                    .WithServiceAllInterfaces()
                    .Configure(c => c.LifeStyle.Transient.Interceptors<AutoReleaseCommandHandlerInterceptor>()),
                Component.For<ICommandDispatcher>().ImplementedBy<CommandDispatcher>().LifestyleTransient(),
                Component.For<ICommandHandlerFactory>().AsFactory(c => c.SelectedWith<CommandHandlerSelector>())
                );

            container.Register(
                Component.For<ITypedFactoryComponentSelector>().ImplementedBy<EventHandlerSelector>(),
                Component.For<AutoReleaseEventHandlerInterceptor>(),
                Classes.FromThisAssembly().BasedOn(typeof (ISubscribeTo<>))
                    .WithServiceAllInterfaces()
                    .Configure(c => c.LifeStyle.Transient.Interceptors<AutoReleaseEventHandlerInterceptor>()),
                Component.For<IEventPublisher>().ImplementedBy<EventPublisherImpl>(),
                Component.For<IEventHandlerFactory>().AsFactory(c => c.SelectedWith<EventHandlerSelector>())
                );

            container.Register(
                Classes.FromThisAssembly().BasedOn<IdentityAndAccess.Queries.ReadModelReaderBase>()
                    .OrBasedOn(typeof (Shop.Queries.Models.ReadModelReaderBase))
                    .WithServiceAllInterfaces()
                    .LifestyleTransient());

            container.Register(Component.For<INotificationLogFactory>().ImplementedBy<NotificationLogFactory>());
            container.Register(Component.For<IEventSerializer>().ImplementedBy<EventSerializer>());

            container.Register(
                Classes.FromThisAssembly().Where(p => p.Name.EndsWith("Service")).WithServiceDefaultInterfaces());
            container.Register(
                Classes.FromThisAssembly().Where(p => p.Name.EndsWith("Store")).WithServiceDefaultInterfaces());

            // TODO: Sollte entfernt werden
            container.Register(
                Classes.FromThisAssembly().Where(p => p.Name.EndsWith("ReadModel")).WithServiceAllInterfaces());

            EventPublisher.Container = container;
        }
    }
}