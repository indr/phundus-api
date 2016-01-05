namespace phiNdus.fundus.Web.App_Start
{
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using Castle.Facilities.AutoTx;
    using Castle.Facilities.NHibernate;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Elmah.Mvc;
    using Phundus.Core.Ddd;
    using Phundus.Persistence;
    using Plumbing;

    public class ContainerConfig : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ElmahController>().ImplementedBy<ElmahController>().LifestylePerWebRequest());

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

            container.Register(Classes.FromThisAssembly().BasedOn(typeof (ISubscribeTo<>))
                .WithServiceAllInterfaces()
                .Configure(c => c.LifeStyle.Transient.Interceptors<AutoReleaseEventHandlerInterceptor>()));

            container.Register(Component.For<IPrincipal>()
                .LifestylePerWebRequest()
                .UsingFactoryMethod(() => HttpContext.Current.User));

            container.Register(Component.For<IIdentity>()
                .LifestylePerWebRequest()
                .UsingFactoryMethod(() => HttpContext.Current.User.Identity));
        }
    }
}