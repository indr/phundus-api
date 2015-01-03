namespace Phundus.Persistence
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common.Domain.Model;
    using SagaStoredEvents;

    public class SagaRepositoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ISagaRepository>().ImplementedBy<SagaRepository>());
        }
    }
}