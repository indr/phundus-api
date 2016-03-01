namespace Phundus.Tests.Cqrs
{
    using Castle.Windsor;
    using Common.Commanding;
    using Machine.Specifications;

    [Subject(typeof (CoreInstaller))]
    public class when_all_handlers_are_resolved
    {
        private static WindsorContainer container;
        private static IHandleCommand<TestCommand1>[] _handlers;

        private Establish context = () =>
        {
            container = new WindsorContainer();
            container.Install(new CoreInstaller(typeof (TestCommand1).Assembly));
        };

        private Because of = () =>
        {
            _handlers = container.ResolveAll<IHandleCommand<TestCommand1>>();
        };

        private It should_have_at_least_one = () =>
            _handlers.Length.ShouldBeGreaterThanOrEqualTo(1);
    }
}