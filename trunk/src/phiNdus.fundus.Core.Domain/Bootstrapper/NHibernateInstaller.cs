using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Bootstrapper
{
    internal class NHibernateInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IUnitOfWorkFactory>()
                                   .Instance(
                                       new NHibernateUnitOfWorkFactory(new[] {Assembly.GetAssembly(typeof (BaseEntity))})));
        }

        #endregion
    }
}