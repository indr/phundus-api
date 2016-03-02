namespace Phundus.Common
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Commanding;
    using Projecting;

    public class ProjectingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //container.Register(Types.FromThisAssembly()
            //    .BasedOn(typeof (IHandleCommand<>))
            //    .WithServiceAllInterfaces()
            //    .Configure(c => c.LifeStyle.Transient.Interceptors<AutoReleaseCommandHandlerInterceptor>()));

            container.Register(Component.For<IProjectionProcessor>()
                .ImplementedBy<ProjectionProcessor>());
        }
    }
}