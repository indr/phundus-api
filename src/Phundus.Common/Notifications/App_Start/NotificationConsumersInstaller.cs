namespace Phundus.Common.Notifications.App_Start
{
    using System.Reflection;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Injecting;

    public class NotificationConsumersInstaller : AssemblyInstallerBase
    {
        public override void Install(IWindsorContainer container, Assembly assembly)
        {
            container.Register(Classes.FromAssembly(assembly)
                .BasedOn<INotificationHandler>()
                .WithServiceAllInterfaces()
                .LifestyleTransient());
        }
    }
}