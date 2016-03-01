namespace Phundus.Common.Tests.Domain.Model
{
    using System;
    using System.Reflection;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public class TestId : GuidIdentity
    {
        public TestId(Guid id) : base(id)
        {
        }
    }

    [Subject("GuidConverter")]
    public class when_converting_from_string : Observes<GuidConverter<TestId>>
    {
        private static object result;
        private static string guidString = "417841a1-c593-4974-aaea-1990ffca7279";

        private Because of = () =>
            result = sut.ConvertFrom(guidString);

        private It should_have_the_guid = () =>
            (result as TestId).Id.ShouldEqual(new Guid(guidString));

        private It should_return_id = () =>
            result.ShouldBeAn<TestId>();
    }

    [Subject("GuidConverter")]
    public class when_converting_an_empty_guid : Observes<GuidConverter<TestId>>
    {
        private Because of = () =>
            spec.catch_exception(() =>
                sut.ConvertFrom(Guid.Empty.ToString("D")));

        private It should_throw_target_invocation_exception = () =>
            spec.exception_thrown.ShouldBeAn<TargetInvocationException>();

        private It should_throw_with_inner_exception_argument_null_exception = () =>
            spec.exception_thrown.InnerException.ShouldBeAn<ArgumentNullException>();
    }

    public class TestIdWithoutCtor : GuidIdentity
    {
    }

    [Subject("GuidConverter")]
    public class when_converting_with_an_id_that_has_no_public_constructor : Observes<GuidConverter<TestIdWithoutCtor>>
    {
        private Because of = () =>
            spec.catch_exception(() =>
                sut.ConvertFrom("5e489c02-371a-42ca-9c9e-26e9b3b07a87"));

        private It should_have_exception_message = () =>
            spec.exception_thrown.Message.ShouldEqual(
                "Could not convert to TestIdWithoutCtor. The type has no public constructor with a Guid argument.");

        private It should_throw_exception = () =>
            spec.exception_thrown.ShouldBeAn<Exception>();
    }
}