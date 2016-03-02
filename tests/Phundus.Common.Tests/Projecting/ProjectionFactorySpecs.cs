namespace Phundus.Common.Tests.Projecting
{
    using System;
    using Common.Projecting;
    using Machine.Specifications;

    public class ProjectionFactorySpecsTestProjection : ProjectionBase<Object>
    {
    }

    [Subject(typeof (IProjectionFactory))]
    public class when_getting_projection_by_full_name : windsor_installer_concern<ProjectingInstaller>
    {
        private static IProjection resolved;

        private Establish ctx = () =>
            new ProjectionsInstaller().Install(container, typeof (ProjectionFactorySpecsTestProjection).Assembly);

        private Because of = () =>
            resolved = resolve<IProjectionFactory>().GetProjection(typeof (ProjectionFactorySpecsTestProjection).FullName);

        private It should_return_test_projection = () =>
            resolved.ShouldBeOfExactType<ProjectionFactorySpecsTestProjection>();
    }
}