namespace Phundus.Common.Tests.Projecting
{
    using Castle.MicroKernel;
    using Common.Projecting;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;

    [Subject(typeof (ITypedProjectionFactory))]
    public class when_getting_projection_by_full_name : windsor_installer_concern<ProjectingInstaller>
    {
        private static IProjection resolved;

        private Establish ctx = () =>
            new ProjectionsInstaller().Install(container, typeof (ProjectionFactorySpecsTestProjection).Assembly);

        private Because of = () =>
            resolved = resolve<ITypedProjectionFactory>()
                .GetProjection(typeof (ProjectionFactorySpecsTestProjection).FullName);

        private It should_return_test_projection = () =>
            resolved.ShouldBeOfExactType<ProjectionFactorySpecsTestProjection>();
    }

    [Subject(typeof (ITypedProjectionFactory))]
    public class when_getting_non_existing_projection : windsor_installer_concern<ProjectingInstaller>
    {
        private Because of = () =>
            spec.catch_exception(() =>
                resolve<ITypedProjectionFactory>()
                    .GetProjection("NotExisting"));

        private It should_throw_component_not_found_exception = () =>
            spec.exception_thrown.ShouldBeAn<ComponentNotFoundException>();
    }
}