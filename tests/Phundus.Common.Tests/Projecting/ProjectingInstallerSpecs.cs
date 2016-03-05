namespace Phundus.Common.Tests.Projecting
{
    using Common.Eventing;
    using Common.Notifications;
    using Common.Projecting;
    using Machine.Specifications;

    [Subject(typeof (ProjectingInstaller))]
    public class ProjectingInstallerSpecs : windsor_installer_concern<ProjectingInstaller>
    {
        private Establish ctx = () =>
        {
            register<IEventStore>();
            register<IProcessedNotificationTrackerStore>();
        };

        private It should_resolve_IProjectionFactory = () =>
            resolve<IProjectionFactory>().ShouldNotBeNull();        

        private It should_resolve_ITypedProjectionFactory = () =>
            resolve<ITypedProjectionFactory>().ShouldNotBeNull();
    }
}