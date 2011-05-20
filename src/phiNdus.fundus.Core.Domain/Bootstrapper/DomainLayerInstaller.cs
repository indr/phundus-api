using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Bootstrapper
{
    public class DomainLayerInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Register Repositories
            container.Register(AllTypes.FromThisAssembly()
                                   .BasedOn(typeof (IRepository<>))
                                   .WithService.AllInterfaces()
                                   .Configure(c => c.LifeStyle.Transient));

            // Register Unit of WOrk / NHibernate
            IoC.Container.Register(Component.For<IUnitOfWorkFactory>().Instance(
                new NHibernateUnitOfWorkFactory(new[] {Assembly.GetAssembly(typeof (BaseEntity))})));
        }

        #endregion
    }
}