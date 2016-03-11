namespace Phundus.Common.Mailing.Installers
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class MailingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IMailGateway>().ImplementedBy<MailGateway>(),
                Component.For<IMessageFactory>().ImplementedBy<MessageFactory>());
        }
    }
}