using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using phiNdus.fundus.Core.Web.State;

namespace phiNdus.fundus.Core.Web.Installers {
    public class StateManagerInstaller : IWindsorInstaller {
        public void Install(IWindsorContainer container, IConfigurationStore store) {
            container.Register(Component.For<IStateManager>().ImplementedBy<SessionStateManager>());
        }
    }
}