namespace Phundus.Persistence
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common.Eventing;
    using Common.Notifications;
    using Inventory.Repositories;
    using Phundus.Inventory.AvailabilityAndReservation.Repositories;
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

            container.Register(Types.FromThisAssembly()
                .BasedOn(typeof (NhRepositoryBase<>))
                .WithServiceAllInterfaces()
                .LifestyleTransient());

            container.Register(Types.FromThisAssembly()
                .BasedOn<EventSourcedRepositoryBase>()
                .WithServiceAllInterfaces()
                .LifestyleTransient());

            container.Register(Component.For<IReservationRepository>()
                .ImplementedBy<NhReservationsBasedOnOrdersRepository>()
                .LifestyleTransient());
        }
    }
}