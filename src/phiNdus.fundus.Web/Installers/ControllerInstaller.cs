using System;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace phiNdus.fundus.Web.Installers
{
    public class ControllerInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly()
                                   .BasedOn<IController>()
                                   .If(Component.IsInNamespace("phiNdus.fundus.Web.Controllers", true))
                                   .If(t => t.Name.EndsWith("Controller", StringComparison.InvariantCulture))
                                   .Configure(c => c.LifeStyle.Transient));
        }

        #endregion
    }
}