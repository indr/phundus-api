namespace Phundus.Core.Tests.Cqrs
{
    using System;
    using System.Collections;
    using Core.Cqrs;

    public class TestCommand1 : ICommand
    {
    }

    public class TestCommand2 : ICommand
    {
    }

    public class TestHandler : IHandleCommand<TestCommand1>, IHandleCommand<TestCommand2>
    {
        #region IHandleCommand<TestCommand1,TestAggregate> Members

        public IEnumerable Handle(TestCommand1 command)
        {
            throw new NotImplementedException("TestHandler1.Handle(TestCommand1)");
        }

        #endregion

        #region IHandleCommand<TestCommand2,TestAggregate> Members

        public IEnumerable Handle(TestCommand2 command)
        {
            throw new NotImplementedException("TestHandler1.Handle(TestCommand2)");
        }

        #endregion

        public IEnumerable Handle(Func<Guid, object> al, TestCommand1 c)
        {
            throw new NotImplementedException("TestHandler1.Handle(Func<Guid, TestAggregate>, TestCommand1)");
        }

        public IEnumerable Handle(Func<Guid, object> al, TestCommand2 c)
        {
            throw new NotImplementedException("TestHandler1.Handle(Func<Guid, TestAggregate>, TestCommand2)");
        }

        public void Execute()
        {
            throw new NotImplementedException("TestHandler1.Execute()");
        }
    }
}