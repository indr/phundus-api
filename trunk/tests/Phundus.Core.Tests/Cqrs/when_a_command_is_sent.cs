namespace Phundus.Tests.Cqrs
{
    using System;
    using Castle.Core.Logging;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Common.Commanding;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    [Subject(typeof (CommandDispatcher))]
    public class when_a_command_is_sent : Observes
    {
        private static ICommandDispatcher dispatcher;
        private static Exception _expectedException;

        private Establish context = () =>
        {
            var container = new WindsorContainer();
            container.AddFacility<TypedFactoryFacility>();
            container.Install(new CommandingInstaller());
            new CommandHandlerInstaller().Install(container, typeof (TestCommand1).Assembly);
            container.Register(Component.For<ILogger>().Instance(fake.an<ILogger>()));

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