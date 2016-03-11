namespace Phundus.Common.Tests.Commanding
{
    using System;
    using Common.Commanding;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public class ConcreteAsyncCommand : AsyncCommand
    {
    }

    [Subject(typeof (AsyncCommand))]
    public class when_constructing_an_async_command : Observes<ConcreteAsyncCommand>
    {
        private It should_have_command_id = () =>
            sut.CommandId.ShouldNotEqual(Guid.Empty);

        private It should_have_created_at_utc = () =>
            sut.CreatedAtUtc.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }
}