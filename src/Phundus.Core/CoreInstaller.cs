namespace Phundus.Core
{
    using System.Reflection;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
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
                Component.For<EventPublisherImpl>(),
                Component.For<IEventHandlerFactory>().AsFactory(c => c.SelectedWith<EventHandlerSelector>())
                );

            container.Register(
                Classes.FromThisAssembly().Where(p => p.Name.EndsWith("ReadModel")).WithServiceAllInterfaces());

            container.Register(
                Classes.FromThisAssembly().Where(p => p.Name.EndsWith("Service")).WithServiceDefaultInterfaces());

            EventPublisher.Container = container;
        }
    }
}