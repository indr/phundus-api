namespace Phundus.Common.Tests.Mailing.Installers
{
    using Common.Mailing;
    using Common.Mailing.Installers;
    using Machine.Specifications;

    [Subject(typeof (MailingInstaller))]
    public class MailingInstallerSpecs : windsor_installer_concern<MailingInstaller>
    {
        private It should_resolve_IMailGateway = () =>
            resolve<IMailGateway>().ShouldBeOfExactType<MailGateway>();

        private It should_resolve_IMessageFactory = () =>
            resolve<IMessageFactory>().ShouldBeOfExactType<MessageFactory>();
    }
}