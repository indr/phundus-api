﻿namespace phiNdus.fundus.Web
{
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Castle.Windsor;
    using phiNdus.fundus.Web.App_Start;

    public class MvcApplication : HttpApplication
    {
        static IWindsorContainer _container;

        protected void Application_Start()
        {
            FileSystemConfig.CreateMissingDirectory(s => Server.MapPath(s));
            DatabaseMigrator.Migrate(s => Server.MapPath(s));

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            _container = ContainerConfig.Bootstrap();
        }

        protected void Application_End()
        {
            if (_container != null)
                _container.Dispose();
        }
    }
}