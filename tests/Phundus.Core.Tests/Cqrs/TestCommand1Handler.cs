namespace Phundus.Core.Tests.Cqrs
{
    using System;
    using System.Collections;
    using Core.Cqrs;

    public class TestCommand1Handler : IHandleCommand<TestCommand1>
    {
        public IEnumerable Handle(TestCommand1 command)
        {
            throw new Exception("TestCommand1Handler.Handle(TestCommand1)");
        }
    }
}