using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using phiNdus.fundus.Core.Web.Models;

namespace phiNdus.fundus.Core.Web.Installers {
    public class AuthenticationInstaller : IWindsorInstaller {
        public void Install(IWindsorContainer container, IConfigurationStore store) {
            container.Register(Component.For<IMembershipService>().ImplementedBy<MembershipService>());
            container.Register(Component.For<IFormsService>().ImplementedBy<FormsAuthenticationService>());
        }
    }
}