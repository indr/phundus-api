namespace Phundus.Core.Tests.Cqrs
{
    using System;
    using System.ComponentModel;
    using Castle.Facilities.FactorySupport;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel;
    using Castle.Windsor;
    using Core.Cqrs;
    using Machine.Specifications;

    public class command_handler_concern
    {
        protected static WindsorContainer container;
        
        Establish context = () =>
        {
            container = new WindsorContainer();
            container.AddFacility<TypedFactoryFacility>();
            container.Install(new CoreInstaller(typeof (TestCommand1).Assembly));
        };
    }

    [Subject(typeof (CoreInstaller))]
    public class when_all_handlers_are_resolved : command_handler_concern
    {
        protected static IHandleCommand<TestCommand1>[] _handlers;

        private Because of = () =>
        {
            _handlers = container.ResolveAll<IHandleCommand<TestCommand1>>();
        };

        private It should_have_at_least_one = () => _handlers.Length.ShouldBeGreaterThanOrEqualTo(1);
    }

    [Subject(typeof(CommandDispatcher))]
    public class when_a_command_with_no_existing_handler_is_dispatched : command_handler_concern
    {
        private static ICommandDispatcher Dispatcher;
        private static Exception Exception;

        private Establish ctx = () => Dispatcher = container.Resolve<ICommandDispatcher>();

        private Because of = () => Exception = Catch.Exception(() => Dispatcher.Dispatch(new TestCommandWithoutHandler()));

        private It should_fail = () => Exception.ShouldBeOfExactType<ComponentNotFoundException>();
    }
}