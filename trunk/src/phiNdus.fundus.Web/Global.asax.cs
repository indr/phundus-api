using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using phiNdus.fundus.DbMigrations;
using phiNdus.fundus.Web.Helpers;
using phiNdus.fundus.Web.Plumbing;
using Rhino.Commons;

namespace phiNdus.fundus.Web
{
    using System;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        private static IWindsorContainer _container;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new InvalidSessionKeyAttribute());
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = ControllerNames.Home, action = "Index", id = UrlParameter.Optional}
                // Parameter defaults
                );

            routes.MapRoute("ImageStore",
                            "{controller}/{action}/{id}/{name}");
        }


        protected void Application_Start()
        {
            CreateMissingDirectory();
            MigrateDatabase();
            PopulateDatabase();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BootstrapContainer();
        }

        // TODO: Gibt es diese Methode in ASP.NET MVC 4?
        protected void Application_End()
        {
            _container.Dispose();
        }

        private static void BootstrapContainer()
        {
            // Locking?
            _container = new WindsorContainer()
                .Install(FromAssembly.This())
                .Install(FromAssembly.Named("phiNdus.fundus.Business"));

            // HttpContext registrieren für den SessionStateManager
            _container.Register(
                Component.For<HttpContextBase>()
                    .LifeStyle
                    .PerWebRequest
                    .UsingFactoryMethod(() => new HttpContextWrapper(HttpContext.Current)));

            var controllerFactory = new WindsorControllerFactory(_container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            IoC.Initialize(_container);
        }

        private void CreateMissingDirectory()
        {
            Directory.CreateDirectory(Server.MapPath(@"~\App_Data\Logs"));
            Directory.CreateDirectory(Server.MapPath(@"~\Content\Images\Articles"));
        }

        private void MigrateDatabase()
        {
            using (var writer = new StreamWriter(Server.MapPath(@"~\App_Data\Logs\DbMigration.log"), true))
            {
                Runner.MigrateToLatest(ConfigurationManager.ConnectionStrings["phundus"].ConnectionString, writer);
            }
        }

        private static void PopulateDatabase()
        {
        }
    }
}