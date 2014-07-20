namespace Phundus.Persistence
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Core.Entities;
    using Core.Repositories;
    using phiNdus.fundus.Domain.Repositories;

    public class Installer : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Types.FromThisAssembly()
                                   //.BasedOn(typeof (IRepository<>))
                                    .BasedOn(typeof (RepositoryBase<>))
                                    .WithServiceAllInterfaces()
                                    .LifestyleTransient());

            //container.Register(Component.For<IMemberRepository>()
            //                            .ImplementedBy<MemberRepository>().LifestyleTransient());

            container.Register(Component.For<IReservationRepository>()
                                        .ImplementedBy<ReservationRepository>()
                                        .LifestyleTransient());

            //container.Register(Component.For<IUnitOfWorkFactory>()
            //                       .Instance(
            //                           new NHibernateUnitOfWorkFactory(new[] {Assembly.GetAssembly(typeof (EntityBase))})));
        }

        #endregion

        
    }
}