namespace Phundus.Persistence
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common.Events;
    using Common.Notifications;
    using Core.Inventory.AvailabilityAndReservation.Repositories;
    using Inventory.Repositories;
    using SagaStoredEvents;
    using StoredEvents;

    public class TrackerStoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IProcessedNotificationTrackerStore>()
                .ImplementedBy<NHibernateProcessedNotificationTrackerStore>()
                .LifestyleTransient());
        }
    }

    public class PersistenceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IEventStore>().ImplementedBy<EventStore>());
            container.Register(Component.For<ISagaEventStore>().ImplementedBy<SagaEventStore>());

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