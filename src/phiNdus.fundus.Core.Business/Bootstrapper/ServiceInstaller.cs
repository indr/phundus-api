using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;

namespace phiNdus.fundus.Core.Business.Bootstrapper {
    public class ServiceInstaller : IWindsorInstaller {

        public void Install(IWindsorContainer container, IConfigurationStore store) {
            container.Register(AllTypes.FromThisAssembly()
                .BasedOn<BaseSecuredService>()
                .WithService.AllInterfaces()
                .Configure(c => c.LifeStyle.Transient));
        }
    }
}
