using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using phiNdus.fundus.Core.Web.Plumbing;
using Castle.MicroKernel.Registration;

namespace phiNdus.fundus.Core.Web {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {

        private static IWindsorContainer container;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute("ImageStore",
                            "{controller}/{action}/{id}/{name}");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822", MessageId = "Mark members as static",
            Justification = "Kann nicht geändert werden, da vom Framework so vorgegeben.")]
        protected void Application_Start() {

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BootstrapContainer();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822", MessageId = "Mark members as static",
            Justification = "Kann nicht geändert werden, da vom Framework so vorgegeben.")]
        // Gemäss Tutorial, aber diese Methode gibts ja nicht, deshalb Dispose
        protected void Application_End() {
            container.Dispose();
        }

        private static void BootstrapContainer() {
            // Locking?
            container = new WindsorContainer()
                .Install(FromAssembly.This())
                .Install(FromAssembly.Named("phiNdus.fundus.Core.Business"));

            // HttpContext registrieren für den SessionStateManager
            container.Register(
                Component.For<HttpContextBase>()
                .LifeStyle
                .PerWebRequest
                .UsingFactoryMethod(() => new HttpContextWrapper(HttpContext.Current)));

            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            Rhino.Commons.IoC.Initialize(container);
        }
    }
}