namespace Phundus.Web
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Bootstrap;
    using Bootstrap.Extensions.StartupTasks;
    using Bootstrap.Windsor;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using IdentityAccess.Organizations.Repositories;
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
            FileSystemConfig.CreateMissingDirectory();
            DatabaseMigrator.Migrate();

            _container = new WindsorContainer();

            Bootstrapper.With.Windsor().WithContainer(_container).And.StartupTasks().Start();

            _container.Register(Component.For<CustomMembershipProvider>()
                .Named("MembershipProvider").LifestyleTransient());
            _container.Register(Component.For<CustomRoleProvider>()
                .Named("RoleProvider").LifestyleTransient());

            var organizations = _container.Resolve<IOrganizationRepository>().FindAll();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes, organizations);
        }

        protected void Application_End()
        {
            if (_container != null)
                _container.Dispose();
        }
    }
}