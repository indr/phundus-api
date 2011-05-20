using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace phiNdus.fundus.Core.Business.Bootstrapper
{
    internal class GatewaysInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMailGateway>()
                                   .ImplementedBy(typeof (MailGateway))
                                   .LifeStyle.Transient);
        }

        #endregion
    }
}