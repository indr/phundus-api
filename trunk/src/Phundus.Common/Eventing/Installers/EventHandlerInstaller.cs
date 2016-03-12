namespace Phundus.Common.Eventing.Installers
{
    using System.Reflection;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Injecting;

    public class EventHandlerInstaller : AssemblyInstallerBase
    {
        public override void Install(IWindsorContainer container, Assembly assembly)
        {
            container.Register(
                Classes.FromAssembly(assembly)
                    .BasedOn(typeof (ISubscribeTo<>))
                    .WithServiceAllInterfaces()
                    .Configure(c => c.LifeStyle.Transient.Interceptors<AutoReleaseEventHandlerInterceptor>()));
        }
    }
}