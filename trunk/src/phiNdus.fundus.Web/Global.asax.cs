namespace Phundus.Web
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Bootstrap;
    using Bootstrap.Extensions.StartupTasks;
    using Bootstrap.Windsor;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Common;
    using Elmah;
    using Rest.Auth;
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

        private void ErrorLog_Filtering(object sender, ExceptionFilterEventArgs args)
        {
            Filter(args);
        }

        private void ErrorMail_Filtering(object sender, ExceptionFilterEventArgs args)
        {
            Filter(args);
        }

        private static void Filter(ExceptionFilterEventArgs args)
        {
            if (ExceptionsToDismiss.Contains(args.Exception.GetBaseException().GetType()))
                args.Dismiss();            
        }

        private static readonly ICollection<Type> ExceptionsToDismiss = new Collection<Type>
        {
            typeof(MaintenanceModeException),
            typeof(NotFoundException)
        };
    }
}