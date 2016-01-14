namespace Phundus.Tests.Cqrs
{
    using System;
    using Phundus.Cqrs;

    public class TestCommand1Handler : IHandleCommand<TestCommand1>
    {
        public void Handle(TestCommand1 command)
        {
            throw new Exception("TestCommand1Handler.Handle(TestCommand1)");
        }
    }
}