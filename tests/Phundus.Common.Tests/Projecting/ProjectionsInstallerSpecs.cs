namespace Phundus.Common.Tests.Projecting
{
    using Common.Projecting;
    using Common.Projecting.Installers;
    using Machine.Specifications;

    [Subject(typeof (ProjectionsInstaller))]
    public class when_installing_projections_installer :
        assembly_installer_concern<ProjectionsInstaller, TestProjection>
    {
        private It should_resolve_IProjection_to_TestProjection = () =>
            resolveAll<IProjection>().ShouldContain(c => c.GetType() == typeof (TestProjection));
    }
}