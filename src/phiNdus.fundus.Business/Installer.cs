using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using phiNdus.fundus.Business.Gateways;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Business.Security;

namespace phiNdus.fundus.Business
{
    public class Installer : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Types.FromThisAssembly()
                                   .BasedOn<SecuredServiceBase>()
                                   .WithService.AllInterfaces()
                                   .LifestyleTransient());

            container.Register(Component.For<Secured>()
                                   .ImplementedBy(typeof (Secured))
                                   .LifeStyle.Transient);

            container.Register(Component.For<IMailGateway>()
                                   .ImplementedBy(typeof (MailGateway))
                                   .LifeStyle.Transient);

            container.Install(new Domain.Installer());
        }

        #endregion
    }
}