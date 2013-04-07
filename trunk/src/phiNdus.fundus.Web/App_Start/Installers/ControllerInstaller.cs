namespace phiNdus.fundus.Web.App_Start.Installers
{
    using System;
    using System.Web.Http.Controllers;
    using System.Web.Mvc;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class ControllerInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Types.FromThisAssembly()
                                    .BasedOn<IController>()
                                    .If(Component.IsInNamespace("phiNdus.fundus.Web.Controllers", true))
                                    .If(t => t.Name.EndsWith("Controller", StringComparison.InvariantCulture))
                                    .LifestyleTransient());

            container.Register(Types.FromThisAssembly()
                                    .BasedOn<IHttpController>()
                                    .If(Component.IsInNamespace("phiNdus.fundus.Web.Controllers.WebApi", true))
                                    .If(t => t.Name.EndsWith("Controller", StringComparison.InvariantCulture))
                                    .LifestyleScoped());
        }

        #endregion
    }
}