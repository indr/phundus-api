using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using phiNdus.fundus.Web.Plumbing;
using piNuts.phundus.Infrastructure;

namespace phiNdus.fundus.Web.App_Start
{
    public class ContainerConfig
    {
        public static IWindsorContainer Bootstrap()
        {
            // Locking?
            var container = new WindsorContainer()
                .Install(FromAssembly.This())
                .Install(FromAssembly.Named("phiNdus.fundus.Business"))
                .Install(FromAssembly.Named("piNuts.phundus.Infrastructure"));

            // HttpContext registrieren für den SessionStateManager
            container.Register(
                Component.For<HttpContextBase>()
                    .LifeStyle
                    .PerWebRequest
                    .UsingFactoryMethod(() => new HttpContextWrapper(HttpContext.Current)));

            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            GlobalContainer.Initialize(container);

            return container;
        }
    }
}