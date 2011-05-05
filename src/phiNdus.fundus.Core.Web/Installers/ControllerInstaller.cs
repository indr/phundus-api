using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using System.Web.Mvc;

namespace phiNdus.fundus.Core.Web.Installers {
    public class ControllerInstaller : IWindsorInstaller {

        public void Install(IWindsorContainer container, IConfigurationStore store) {
            container.Register(AllTypes.FromThisAssembly()
                                .BasedOn<IController>()
                                .If(Component.IsInNamespace("phiNdus.fundus.Core.Web.Controllers", true))
                                .If(t => t.Name.EndsWith("Controller"))
                                .Configure(c => c.LifeStyle.Transient));
        }
    }
}