namespace Phundus.Common.Infrastructure
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class InfrastructureInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IHostingEnvironment>().ImplementedBy<HostingEnvironmentImpl>(),
                Component.For<IFileStoreFactory>().ImplementedBy<AppDataFileStoreFactory>());
        }
    }
}