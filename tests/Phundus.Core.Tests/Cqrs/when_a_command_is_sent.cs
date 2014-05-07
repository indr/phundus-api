namespace Phundus.Core.Tests.Cqrs
{
    using System;
    using System.Collections;
    using Castle.Windsor;
    using Core.Cqrs;
    using Machine.Specifications;

    [Subject(typeof (CommandDispatcher))]
    public class when_a_command_is_sent
    {
        private static ICommandDispatcher dispatcher;
        private static Exception _expectedException;

        private Establish context =
            () =>
            {
                var container = new WindsorContainer();
                container.Install(new CoreInstaller(typeof(when_a_command_is_sent).Assembly));

                
                dispatcher = container.Resolve<ICommandDispatcher>();
            };

        private Because of =
            () => _expectedException = Catch.Exception(() => dispatcher.Dispatch(new TestCommand1()));
        
        private It should_have_expected_message =
            () => _expectedException.Message.ShouldEqual("TestHandler1.Handle()");

        private It should_throw_Exception =
            () => _expectedException.ShouldNotBeNull();
    }

    public class TestAggregate
    {
    }

    public class TestCommand1 : ICommand
    {
    }

    public class TestHandler1 : ICommandHandler<TestCommand1>
    {
        #region ICommandHandler<TestCommand1> Members

        public void Execute()
        {
            throw new NotImplementedException("TestHandler1.Handle()");
        }

        public TestCommand1 Command { get; set; }

        #endregion

        public IEnumerable Handle(Func<Guid, TestAggregate> al, TestCommand1 c)
        {
            throw new NotImplementedException("TestHandler1.Handle()");
        }
    }
}