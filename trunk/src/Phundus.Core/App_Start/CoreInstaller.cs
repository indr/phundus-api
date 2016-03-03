namespace Phundus
{
    using System.Reflection;
    using Authorization;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common.Commanding;
    using Common.Eventing;
    using Common.Projecting;
    using Common.Projecting.Application;
    using Common.Querying;
    using IdentityAccess.Users.Services;

    public class AssemblyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var assembly = GetType().Assembly;
            new CommandHandlerInstaller().Install(container, assembly);
            new ProjectionsInstaller().Install(container, assembly);
        }
    }

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
            

            container.Register(
                Component.For<ITypedFactoryComponentSelector>().ImplementedBy<AccessObjectHandlerSelector>(),
                Component.For<AutoReleaseAccessObjectHandlerInterceptor>(),
                Types.FromAssembly(_assemblyContainingCommandsAndHandlers)
                    .BasedOn(typeof (IHandleAccessObject<>))
                    .WithServiceAllInterfaces()
                    .Configure(c => c.LifeStyle.Transient.Interceptors<AutoReleaseAccessObjectHandlerInterceptor>()),
                Component.For<IAuthorize>().ImplementedBy<Authorization.Authorize>().LifestyleTransient(),
                Component.For<IAccessObjectHandlerFactory>()
                    .AsFactory(c => c.SelectedWith<AccessObjectHandlerSelector>())
                );

            container.Register(
                Component.For<ITypedFactoryComponentSelector>().ImplementedBy<EventHandlerSelector>(),
                Component.For<AutoReleaseEventHandlerInterceptor>(),
                Classes.FromThisAssembly()
                    .BasedOn(typeof (ISubscribeTo<>))
                    .WithServiceAllInterfaces()
                    .Configure(c => c.LifeStyle.Transient.Interceptors<AutoReleaseEventHandlerInterceptor>()),
                Component.For<IEventPublisher>().ImplementedBy<EventPublisherImpl>(),
                Component.For<IEventHandlerFactory>().AsFactory(c => c.SelectedWith<EventHandlerSelector>())
                );

            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<QueryBase>()
                    .WithServiceAllInterfaces());

            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<ProjectionBase>()
                    .WithServiceAllInterfaces());

            container.Register(
                Component.For<IUserInRole>()
                    .ImplementedBy<UserInRole>());

            container.Register(
                Component.For<Inventory.Model.IUserInRole>()
                    .ImplementedBy<Inventory.Model.UserInRole>());

            container.Register(
                Component.For<Shop.Model.IUserInRole>()
                    .ImplementedBy<Shop.Model.UserInRole>());

            EventPublisher.Factory(container.Resolve<IEventPublisher>);

            container.Register(Component.For<IWindsorContainer>().Instance(container));

            container.Register(
                Classes.FromThisAssembly().Where(p => p.Name.EndsWith("Service")).WithServiceDefaultInterfaces());
            container.Register(
                Classes.FromThisAssembly().Where(p => p.Name.EndsWith("Store")).WithServiceDefaultInterfaces());
        }
    }
}