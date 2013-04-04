using System;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;

namespace phiNdus.fundus.Domain
{
    using piNuts.phundus.Infrastructure.Obsolete;

    public class Installer : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Types.FromAssemblyContaining(typeof (EntityBase))
                                   //.BasedOn(typeof (IRepository<>))
                                   .BasedOn(typeof (NHRepository<>))
                                   .WithServiceAllInterfaces()
                                   .LifestyleTransient());

            container.Register(Component.For<IReservationRepository>()
                                   .ImplementedBy<ReservationRepository>());

            //container.Register(Component.For<IUnitOfWorkFactory>()
            //                       .Instance(
            //                           new NHibernateUnitOfWorkFactory(new[] {Assembly.GetAssembly(typeof (EntityBase))})));
        }

        #endregion
    }
}