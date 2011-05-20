using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using phiNdus.fundus.Core.Domain.Bootstrapper;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Bootstrapper {
    public class BusinessLayerInstaller : IWindsorInstaller {

        public void Install(IWindsorContainer container, IConfigurationStore store) {
            // Install Services
            container.Register(AllTypes.FromThisAssembly()
                .BasedOn<BaseSecuredService>()
                .WithService.AllInterfaces()
                .Configure(c => c.LifeStyle.Transient));
            
            // Install Gateways
            IoC.Container.Register(Component.For<IMailGateway>()
                .ImplementedBy(typeof(MailGateway))
                .LifeStyle.Transient);
            
            // Install Domain Layer
            container.Install(new DomainLayerInstaller());
        }
    }
}
