namespace Phundus.Core
{
    using Castle.Core;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Phundus.Core.Common;

    internal class CoreInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<TypedFactoryFacility>()
                     .Register(
                         Component.For<ITypedFactoryComponentSelector>().ImplementedBy<CommandHandlerSelector>(),
                         Component.For<AutoReleaseHandlerInterceptor>(),
                         Classes.FromThisAssembly()
                                .BasedOn(typeof(ICommandHandler<>))
                                .WithService.Base()
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