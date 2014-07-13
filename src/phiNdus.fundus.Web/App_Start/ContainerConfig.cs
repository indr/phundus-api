namespace phiNdus.fundus.Web.App_Start
{
    using System.Security.Principal;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using Castle.Facilities.AutoTx;
    using Castle.Facilities.NHibernate;
    using Castle.MicroKernel.Registration;
    using Castle.Transactions;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using CommonServiceLocator.WindsorAdapter;
    using FluentNHibernate.Cfg;
    using Microsoft.Practices.ServiceLocation;
    using NHibernate;
    using phiNdus.fundus.Web.Business.Services;
    using phiNdus.fundus.Web.Plumbing;
    using Phundus.Core.Entities;

    public class ContainerConfig
    {
        public static IWindsorContainer Bootstrap()
        {
            // Locking?
            var container = new WindsorContainer()
                .Install(FromAssembly.This())
                .Install(FromAssembly.Named("phiNdus.fundus.Domain"))
                .Install(FromAssembly.Named("piNuts.phundus.Infrastructure"))
                .Install(FromAssembly.Named("Phundus.Core"));


            container.Register(Types.FromThisAssembly()
                                    .BasedOn<BaseService>()
                                    .WithServiceFirstInterface()
                                    .LifestyleTransient());

            // HttpContext registrieren für den SessionStateManager
            //container.Register(
            //    Component.For<HttpContextBase>()
            //        .LifeStyle
            //        .PerWebRequest
            //        .UsingFactoryMethod(() => new HttpContextWrapper(HttpContext.Current)));

            container.AddFacility<AutoTxFacility>();
            container.Register(Component.For<INHibernateInstaller>()
                                        .ImplementedBy<NHibernateInstaller>());
            container.AddFacility<NHibernateFacility>();


            GlobalConfiguration.Configuration.DependencyResolver
                = new WindsorDependencyResolver(container);

            ControllerBuilder.Current.SetControllerFactory(
                new WindsorControllerFactory(container.Kernel));


            container.Register(Component.For<IPrincipal>()
                                        .LifestylePerWebRequest()
                                        .UsingFactoryMethod(() => HttpContext.Current.User));

            container.Register(Component.For<IIdentity>()
                                        .LifestylePerWebRequest()
                                        .UsingFactoryMethod(() => HttpContext.Current.User.Identity));
                

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));

            return container;
        }
    }

    public class NHibernateInstaller : INHibernateInstaller
    {
        private readonly Maybe<IInterceptor> _interceptor;

        public NHibernateInstaller()
        {
            _interceptor = Maybe.None<IInterceptor>();
        }

        #region INHibernateInstaller Members

        public FluentConfiguration BuildFluent()
        {
            var cfg = new NHibernate.Cfg.Configuration();

            var assembly = typeof (EntityBase).Assembly;
            cfg.AddAssembly(assembly);

            return Fluently.Configure(cfg);
        }

        public void Registered(ISessionFactory factory)
        {
            //
        }

        public bool IsDefault
        {
            get { return true; }
        }

        public string SessionFactoryKey
        {
            get { return "sf.default"; }
        }

        public Maybe<IInterceptor> Interceptor
        {
            get { return _interceptor; }
        }

        #endregion
    }
}