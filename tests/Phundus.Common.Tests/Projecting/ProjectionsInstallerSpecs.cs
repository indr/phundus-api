namespace Phundus.Common.Tests.Projecting
{
    using System;
    using Common.Domain.Model;
    using Common.Projecting;
    using Machine.Specifications;

    public class TestProjection : ProjectionBase<Object>
    {
        public override void Handle(DomainEvent e)
        {
            throw new NotImplementedException();
        }
    }

    [Subject(typeof (ProjectionsInstaller))]
    public class when_installing_projections_installer :
        assembly_installer_concern<ProjectionsInstaller, TestProjection>
    {
        private It should_resolve_IProjection_to_TestProjection = () =>
            resolveAll<IProjection>().ShouldContain(c => c.GetType() == typeof (TestProjection));
    }
}