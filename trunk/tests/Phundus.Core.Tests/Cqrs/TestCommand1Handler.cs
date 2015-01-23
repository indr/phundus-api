namespace Phundus.Core.Tests.Cqrs
{
    using System;
    using Common.Cqrs;
    using Core.Cqrs;

    public class TestCommand1 : ICommand
    {
    }

    public class TestCommand1Handler : IHandleCommand<TestCommand1>
    {
        public void Handle(TestCommand1 command)
        {
            throw new Exception("TestCommand1Handler.Handle(TestCommand1)");
        }
    }

    public class TestCommandWithoutHandler : ICommand
    {
    }
}