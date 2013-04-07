namespace phiNdus.fundus.Domain
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Entities;
    using Repositories;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class Installer : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Types.FromAssemblyContaining(typeof (EntityBase))
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