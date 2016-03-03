namespace Phundus.Common.Tests.Projecting
{
    using System.Linq;
    using Castle.MicroKernel;
    using Common.Projecting;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;

    [Subject(typeof (ITypedProjectionFactory))]
    public class get_projections_from_typed_projection_factory : windsor_installer_concern<ProjectingInstaller>
    {
        private static IProjection[] resolved;

        private Establish ctx = () =>
            new ProjectionsInstaller().Install(container, typeof(get_projections_from_typed_projection_factory));

        private Because of = () =>
            resolved = resolve<ITypedProjectionFactory>()
                .GetProjections();

        private It should_return_projections = () =>
            resolved.Count().ShouldBeGreaterThan(0);
    }

    [Subject(typeof (ITypedProjectionFactory))]
    public class get_projection : windsor_installer_concern<ProjectingInstaller>
    {
        private static IProjection resolved;

        private Establish ctx = () =>
            new ProjectionsInstaller().Install(container, typeof (get_projection));

        private Because of = () =>
            resolved = resolve<ITypedProjectionFactory>()
                .GetProjection(typeof (ProjectionFactorySpecsTestProjection).FullName);

        private It should_return_test_projection = () =>
            resolved.ShouldBeOfExactType<ProjectionFactorySpecsTestProjection>();
    }

    [Subject(typeof (ITypedProjectionFactory))]
    public class get_projection_when_projection_does_not_exist : windsor_installer_concern<ProjectingInstaller>
    {
        private Because of = () =>
            spec.catch_exception(() =>
                resolve<ITypedProjectionFactory>()
                    .GetProjection("NotExisting"));

        private It should_throw_component_not_found_exception = () =>
            spec.exception_thrown.ShouldBeAn<ComponentNotFoundException>();
    }
}