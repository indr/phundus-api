namespace Phundus.Common.Tests.Projecting
{
    using Common.Projecting;
    using Machine.Specifications;

    [Subject(typeof (ProjectingInstaller))]
    public class ProjectingInstallerSpecs : windsor_installer_concern<ProjectingInstaller>
    {
        private It should_resolve_IProjectionFactory = () =>
            resolve<IProjectionFactory>().ShouldNotBeNull();
    }
}