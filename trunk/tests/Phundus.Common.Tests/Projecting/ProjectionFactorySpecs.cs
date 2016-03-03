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

    
    [Subject(typeof (ProjectionFactory))]
    public class when_finding_projection_by_full_name : Observes<ProjectionFactory>
    {
        protected static ITypedProjectionFactory typedProjectionFactory;
        protected static IProjection returnValue;

        private Establish ctx = () =>
            typedProjectionFactory = depends.on<ITypedProjectionFactory>();

        private Because of = () =>
            returnValue = sut.FindProjection("fullName");
    }

    [Subject(typeof(ProjectionFactory))]
    public class when_projection_factory_returns_projection : when_finding_projection_by_full_name
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

    [Subject(typeof(ProjectionFactory))]
    public class when_typed_projection_factory_throws_component_not_found_exception : when_finding_projection_by_full_name
    {
        private Establish ctx = () =>
            typedProjectionFactory.setup(x => x.GetProjection("fullName"))
                .Throw(new ComponentNotFoundException("", ""));

        private It should_return_null = () =>
            returnValue.ShouldBeNull();
    }
}