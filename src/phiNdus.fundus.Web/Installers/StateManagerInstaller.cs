using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using phiNdus.fundus.Web.State;

namespace phiNdus.fundus.Web.Installers
{
    public class StateManagerInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IStateManager>().ImplementedBy<SessionStateManager>());
        }

        #endregion
    }
}