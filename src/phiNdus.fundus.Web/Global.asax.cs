namespace phiNdus.fundus.Web
{
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Castle.Windsor;
    using phiNdus.fundus.Domain.Repositories;
    using phiNdus.fundus.Web.App_Start;
    using phiNdus.fundus.Web.Controllers.WebApi;

    public class MvcApplication : HttpApplication
    {
        static IWindsorContainer _container;

        protected void Application_Start()
        {
            FileSystemConfig.CreateMissingDirectory(s => Server.MapPath(s));
            DatabaseMigrator.Migrate(s => Server.MapPath(s));

            _container = ContainerConfig.Bootstrap();


            var organizations = _container.Resolve<IOrganizationRepository>().FindAll();

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes, organizations);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_End()
        {
            if (_container != null)
                _container.Dispose();
        }
    }
}