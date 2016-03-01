﻿namespace Phundus.Web
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Bootstrap;
    using Bootstrap.Extensions.StartupTasks;
    using Bootstrap.Windsor;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Security;

    public class MvcApplication : HttpApplication, IContainerAccessor
    {
        private static IWindsorContainer _container;

        public IWindsorContainer Container
        {
            get { return _container; }
        }

        protected void Application_Start()
        {
            FileSystemConfig.CreateMissingDirectory();
            DatabaseMigrator.Migrate();

            _container = new WindsorContainer();

            _container.Register(Component.For<CustomMembershipProvider>()
                .Named("MembershipProvider").LifestyleTransient());
            _container.Register(Component.For<CustomRoleProvider>()
                .Named("RoleProvider").LifestyleTransient());

            Bootstrapper.With.Windsor().WithContainer(_container)
                .And.StartupTasks()
                .Start();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_End()
        {
            if (_container != null)
                _container.Dispose();
        }
    }
}