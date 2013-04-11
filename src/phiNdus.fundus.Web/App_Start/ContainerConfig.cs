using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using phiNdus.fundus.Web.Plumbing;
using piNuts.phundus.Infrastructure;

namespace phiNdus.fundus.Web.App_Start
{
    using System;
    using System.Reflection;
    using System.Text;
    using System.Web.Http;
    using Business.Services;
    using Castle.Facilities.AutoTx;
    using Castle.Facilities.NHibernate;
    using Castle.Transactions;
    using FluentNHibernate.Cfg;
    using NHibernate;
    using phiNdus.fundus.Domain.Entities;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class ContainerConfig
    {
        public static IWindsorContainer Bootstrap()
        {
            // Locking?
            var container = new WindsorContainer()
                .Install(FromAssembly.This())
                .Install(FromAssembly.Named("phiNdus.fundus.Domain"))
                .Install(FromAssembly.Named("piNuts.phundus.Infrastructure"));


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
            var cfg = new NHibernate.Cfg.Configuration();

            var assembly = typeof (EntityBase).Assembly;
            cfg.AddAssembly(assembly);

            return Fluently.Configure(cfg);
        }

        public void Registered(ISessionFactory factory)
        {
            //
        }

        public bool IsDefault { get { return true; } }
        public string SessionFactoryKey { get { return "sf.default"; } }
        public Maybe<IInterceptor> Interceptor { get { return _interceptor; } }
    }
}