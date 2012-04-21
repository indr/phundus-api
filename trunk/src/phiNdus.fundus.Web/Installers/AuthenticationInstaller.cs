using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using phiNdus.fundus.Web.Models;

namespace phiNdus.fundus.Web.Installers
{
    public class AuthenticationInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMembershipService>().ImplementedBy<MembershipService>());
            container.Register(Component.For<IFormsService>().ImplementedBy<FormsAuthenticationService>());
        }

        #endregion
    }
}