namespace Phundus.Core.Organizations
{
    using System.Reflection;
    using Castle.Core;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Infrastructure.Cqrs;

    public class Installer : IWindsorInstaller
    {
        private readonly Assembly _assemblyContainingCommandsAndHandlers;

        public Installer() : this(typeof (Installer).Assembly)
        {
        }

        public Installer(Assembly assemblyContainingCommandsAndHandlers)
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
        }

        #endregion
    }
}