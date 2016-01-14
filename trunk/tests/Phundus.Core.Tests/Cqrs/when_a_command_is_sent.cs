﻿namespace Phundus.Tests.Cqrs
{
    using System;
    using Castle.Facilities.TypedFactory;
    using Castle.Windsor;
    using Machine.Specifications;
    using Phundus.Cqrs;

    [Subject(typeof (CommandDispatcher))]
    public class when_a_command_is_sent
    {
        private static ICommandDispatcher dispatcher;
        private static Exception _expectedException;

        private Establish context = () =>
        {
            var container = new WindsorContainer();
            container.AddFacility<TypedFactoryFacility>();
            container.Install(new CoreInstaller(typeof (when_a_command_is_sent).Assembly));

            dispatcher = container.Resolve<ICommandDispatcher>();
        };

        private Because of =
            () => _expectedException = Catch.Exception(() => dispatcher.Dispatch(new TestCommand1()));

        private It should_have_expected_message =
            () => _expectedException.Message.ShouldEqual("TestCommand1Handler.Handle(TestCommand1)");

        private It should_throw_Exception =
            () => _expectedException.ShouldNotBeNull();
    }
}