namespace phiNdus.fundus.Web
{
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using phiNdus.fundus.Web.App_Start;
    using phiNdus.fundus.Web.Security;
    using Phundus.Core.Repositories;
    using Phundus.Rest;

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
            FileSystemConfig.CreateMissingDirectory();

            StartMitSqlServerCe.TuEs();

            DatabaseMigrator.Migrate();
            AutoMapperConfig.Config();

            _container = ContainerConfig.Bootstrap();
            _container.Register(Component.For<CustomMembershipProvider>()
                                         .Named("MembershipProvider").LifestyleTransient());
            _container.Register(Component.For<CustomRoleProvider>()
                                         .Named("RoleProvider").LifestyleTransient());

            var organizations = _container.Resolve<IOrganizationRepository>().FindAll();

            AreaRegistration.RegisterAllAreas();
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