﻿namespace Phundus.Persistence
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common.Events;
    using Core.Inventory.AvailabilityAndReservation.Repositories;
    using Inventory.Repositories;
    using StoredEvents;

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