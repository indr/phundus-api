namespace Phundus.Core
{
    using System.Reflection;
    using Castle.Core;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Cqrs;

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

        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<TypedFactoryFacility>()
                .Register(
                    Component.For<ITypedFactoryComponentSelector>().ImplementedBy<CommandHandlerSelector>(),
                    Component.For<AutoReleaseHandlerInterceptor>(),
                    Types.FromAssembly(_assemblyContainingCommandsAndHandlers)
                        .BasedOn(typeof (IHandleCommand<>))
                        .WithServiceAllInterfaces()
                        .Configure(
                            c =>
                                c.LifeStyle.Is(LifestyleType.Transient)
                                    .Interceptors<AutoReleaseHandlerInterceptor>()),
                    Component.For<ICommandDispatcher>().ImplementedBy<CommandDispatcher>().LifestyleTransient(),
                    Component.For<ICommandHandlerFactory>().AsFactory(c => c.SelectedWith<CommandHandlerSelector>())
                );

            container.Register(
                Classes.FromThisAssembly().Where(p => p.Name.EndsWith("ReadModel")).WithServiceDefaultInterfaces());
        }

        #endregion
    }
}