using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain
{
    public class Installer : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly()
                                   .BasedOn(typeof (IRepository<>))
                                   .WithService.AllInterfaces()
                                   .Configure(c => c.LifeStyle.Transient));

            if (container.ResolveAll<IUnitOfWorkFactory>().Length == 0)
            {
                container.Register(Component.For<IUnitOfWorkFactory>()
                                       .Instance(
                                           new NHibernateUnitOfWorkFactory(new[] {Assembly.GetAssembly(typeof (Entity))})));
            }
        }

        #endregion
    }
}