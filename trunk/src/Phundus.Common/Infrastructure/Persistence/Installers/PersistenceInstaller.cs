namespace Phundus.Common.Infrastructure.Persistence.Installers
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common.Domain.Model;
    using Common.Eventing;

    public class PersistenceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IEventStore>()
                .ImplementedBy<EventStore>());

            container.Register(Component.For<IShortIdGeneratorService>()
                .ImplementedBy<ShortIdGeneratorService>());
        }
    }
}