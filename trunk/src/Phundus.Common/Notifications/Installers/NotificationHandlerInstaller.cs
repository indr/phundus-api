namespace Phundus.Common.Notifications.Installers
{
    using System.Reflection;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Injecting;

    public class NotificationHandlerInstaller : AssemblyInstallerBase
    {
        public override void Install(IWindsorContainer container, Assembly assembly)
        {
            container.Register(Classes.FromAssembly(assembly)
                .BasedOn(typeof(INotificationHandler))
                .WithServiceAllInterfaces()
                .LifestyleTransient());
        }
    }
}