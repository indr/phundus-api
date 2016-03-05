namespace Phundus.Common.Notifications.Installers
{
    using System.Reflection;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Injecting;

    public class EventConsumerInstaller : AssemblyInstallerBase
    {
        public override void Install(IWindsorContainer container, Assembly assembly)
        {
            container.Register(Classes.FromAssembly(assembly)
                .BasedOn<IEventConsumer>()
                .WithServiceBase()
                .LifestyleTransient());
        }
    }
}