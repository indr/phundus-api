using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using phiNdus.fundus.Web.Plumbing;
using piNuts.phundus.Infrastructure;

namespace phiNdus.fundus.Web.App_Start
{
    using Castle.Facilities.AutoTx;
    using Castle.Facilities.NHibernate;
    using Castle.Transactions;
    using FluentNHibernate.Cfg;
    using NHibernate;

    public class ContainerConfig
    {
        public static IWindsorContainer Bootstrap()
        {
            // Locking?
            var container = new WindsorContainer()
                .Install(FromAssembly.This())
                .Install(FromAssembly.Named("phiNdus.fundus.Business"))
                .Install(FromAssembly.Named("piNuts.phundus.Infrastructure"));

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

            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            GlobalContainer.Initialize(container);

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

        public FluentConfiguration BuildFluent()
        {
            return Fluently.Configure(new NHibernate.Cfg.Configuration());
        }

        public void Registered(ISessionFactory factory)
        {
            
        }

        public bool IsDefault { get { return true; } }
        public string SessionFactoryKey { get { return "sf.default"; } }
        public Maybe<IInterceptor> Interceptor { get { return _interceptor; } }
    }
}