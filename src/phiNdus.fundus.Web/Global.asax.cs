namespace phiNdus.fundus.Web
{
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using App_Start;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Domain.Repositories;
    using Security;

    public class MvcApplication : HttpApplication, IContainerAccessor
    {
        private static IWindsorContainer _container;

        #region IContainerAccessor Members

        public IWindsorContainer Container
        {
            get { return _container; }
        }

        #endregion

        protected void Application_Start()
        {
            FileSystemConfig.CreateMissingDirectory(s => Server.MapPath(s));
            DatabaseMigrator.Migrate(s => Server.MapPath(s));

            _container = ContainerConfig.Bootstrap();
            _container.Register(Component.For<CustomMembershipProvider>()
                                         .Named("MembershipProvider").LifestyleTransient());
            _container.Register(Component.For<CustomRoleProvider>()
                                         .Named("RoleProvider").LifestyleTransient());

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