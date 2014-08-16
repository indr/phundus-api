namespace phiNdus.fundus.Web.App_Start
{
    using System.Security.Principal;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using Castle.Facilities.AutoTx;
    using Castle.Facilities.NHibernate;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using CommonServiceLocator.WindsorAdapter;
    using Microsoft.Practices.ServiceLocation;
    using Phundus.Core;
    using Phundus.Persistence;
    
    using Plumbing;

    public class ContainerConfig
    {
        public static IWindsorContainer Bootstrap()
        {
            IWindsorContainer container = new WindsorContainer()
                .Install(FromAssembly.Named("Phundus.Infrastructure"))
                .Install(FromAssembly.Named("Phundus.Core"))
                .Install(FromAssembly.This())
                .Install(FromAssembly.Named("Phundus.Persistence"))
                .Install(FromAssembly.Named("Phundus.Rest"));


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
}