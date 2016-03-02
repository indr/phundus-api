namespace Phundus.Common.Tests.Projecting
{
    using System;
    using Castle.MicroKernel;
    using Common.Projecting;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public class ProjectionFactorySpecsTestProjection : ProjectionBase<Object>
    {
    }

    [Subject(typeof (ITypedProjectionFactory))]
    public class when_getting_projection_by_full_name : windsor_installer_concern<ProjectingInstaller>
    {
        private static IProjection resolved;

        private Establish ctx = () =>
            new ProjectionsInstaller().Install(container, typeof (ProjectionFactorySpecsTestProjection).Assembly);

        private Because of = () =>
            resolved =
                resolve<ITypedProjectionFactory>().GetProjection(typeof (ProjectionFactorySpecsTestProjection).FullName);

        private It should_return_test_projection = () =>
            resolved.ShouldBeOfExactType<ProjectionFactorySpecsTestProjection>();
    }

    [Subject(typeof (ProjectionFactory))]
    public class when_finding_projection_by_full_name : Observes<ProjectionFactory>
    {
        private static ITypedProjectionFactory typedProjectionFactory;
        private static IProjection returnValue;

        private Establish ctx = () =>
            typedProjectionFactory = depends.on<ITypedProjectionFactory>();

        private Because of = () =>
            returnValue = sut.FindProjection("fullName");

        public class when_typed_projection_factory_returns_projection
        {
            private static IProjection projection;

            private Establish ctx = () =>
            {
                projection = fake.an<IProjection>();
                typedProjectionFactory.setup(x => x.GetProjection("fullName")).Return(projection);
            };

            private It should_return_projection = () =>
                returnValue.ShouldBeTheSameAs(projection);
        }

        public class when_typed_projection_factory_throws_component_not_found_exception
        {
            private Establish ctx = () =>
                typedProjectionFactory.setup(x => x.GetProjection("fullName"))
                    .Throw(new ComponentNotFoundException("", ""));

            private It should_return_null = () =>
                returnValue.ShouldBeNull();
        }
    }
}