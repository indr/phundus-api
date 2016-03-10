namespace Phundus.Common.Notifications.Installers
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class TrackerStoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IProcessedNotificationTrackerStore>()
                .ImplementedBy<ProcessedNotificationTrackerStore>()
                .LifestyleTransient());
        }
    }
}