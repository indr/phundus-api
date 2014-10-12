namespace Phundus.Persistence
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Core.Ddd;
    using Core.Inventory.AvailabilityAndReservation.Repositories;
    using Ddd;
    using Inventory.Repositories;

    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IEventStore>().ImplementedBy<EventStore>());

            container.Register(Types.FromThisAssembly()
                .BasedOn(typeof (NhRepositoryBase<>))
                .WithServiceAllInterfaces()
                .LifestyleTransient());

            container.Register(Component.For<IReservationRepository>()
                .ImplementedBy<NhReservationsBasedOnOrdersRepository>()
                .LifestyleTransient());
        }
    }
}