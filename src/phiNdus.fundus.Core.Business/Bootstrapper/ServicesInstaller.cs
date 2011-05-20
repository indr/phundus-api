using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace phiNdus.fundus.Core.Business.Bootstrapper
{
    internal class ServicesInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly()
                                   .BasedOn<BaseSecuredService>()
                                   .WithService.AllInterfaces()
                                   .Configure(c => c.LifeStyle.Transient));
        }

        #endregion
    }
}