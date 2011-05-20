using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Bootstrapper
{
    internal class RepositoriesInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly()
                                   .BasedOn(typeof (IRepository<>))
                                   .WithService.AllInterfaces()
                                   .Configure(c => c.LifeStyle.Transient));
        }

        #endregion
    }
}