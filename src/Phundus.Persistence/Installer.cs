namespace Phundus.Persistence
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Core.ReservationCtx.Repositories;
    using Inventory.Repositories;

    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Types.FromThisAssembly()
                .BasedOn(typeof (NhRepositoryBase<>))
                .WithServiceAllInterfaces()
                .LifestyleTransient());

            container.Register(Component.For<IReservationRepository>()
                .ImplementedBy<NhReservationRepository>()
                .LifestyleTransient());
        }
    }
}