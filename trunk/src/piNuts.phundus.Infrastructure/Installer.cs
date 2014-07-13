namespace Phundus.Infrastructure
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Gateways;

    public class Installer : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMailGateway>()
                                        .ImplementedBy(typeof(MailGateway))
                                        .LifeStyle.Transient);

            
        }

        #endregion
    }
}