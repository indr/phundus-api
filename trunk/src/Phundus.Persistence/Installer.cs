namespace Phundus.Persistence
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Core.ReservationCtx.Repositories;
    using Repositories;

    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Types.FromThisAssembly()
                .BasedOn(typeof (RepositoryBase<>))
                .WithServiceAllInterfaces()
                .LifestyleTransient());

            container.Register(Component.For<IReservationRepository>()
                .ImplementedBy<ReservationRepository>()
                .LifestyleTransient());
        }
    }
}