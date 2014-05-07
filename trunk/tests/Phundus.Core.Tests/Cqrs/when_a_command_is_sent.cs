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
        private static CommandDispatcher dispatcher;
        private static Exception _expectedException;

        private Establish context =
            () =>
            {
                var container = new WindsorContainer();
                container.Install(new CoreInstaller(typeof (when_a_command_is_sent).Assembly));

                //dispatcher = container.Resolve<ICommandDispatcher>();
                dispatcher = new CommandDispatcher();
                dispatcher.Container = container;
            };

        private Because of =
            () => _expectedException = Catch.Exception(() => dispatcher.Dispatch(new TestCommand1()));

        private It should_have_expected_message =
            () => _expectedException.Message.ShouldEqual("TestHandler1.Handle(TestCommand1)");

        private It should_throw_Exception =
            () => _expectedException.ShouldNotBeNull();
    }

    public class TestAggregate
    {
    }

    public class TestCommand1 : ICommand
    {
    }

    public class TestCommand2 : ICommand
    {
    }

    public class TestHandler1 : IHandleCommand<TestCommand1>, IHandleCommand<TestCommand2>
    {
        #region IHandleCommand<TestCommand1> Members

        public void Handle(TestCommand1 command)
        {
            throw new NotImplementedException("TestHandler1.Handle(TestCommand1)");
        }

        #endregion

        #region IHandleCommand<TestCommand2> Members

        public void Handle(TestCommand2 command)
        {
            throw new NotImplementedException("TestHandler1.Handle(TestCommand2)");
        }

        #endregion

        public void Execute()
        {
            throw new NotImplementedException("TestHandler1.Execute()");
        }

        public IEnumerable Handle(Func<Guid, TestAggregate> al, TestCommand1 c)
        {
            throw new NotImplementedException("TestHandler1.Handle(Func<Guid, TestAggregate>, TestCommand1)");
        }
    }
}