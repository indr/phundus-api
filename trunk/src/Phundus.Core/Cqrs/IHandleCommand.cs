namespace Phundus.Core.Cqrs
{
    using Castle.Transactions;
    using Common.Cqrs;

    public interface IHandleCommand
    {
    }

    public interface IHandleCommand<in TCommand> : IHandleCommand where TCommand : ICommand
    {
        [Transaction]
        void Handle(TCommand command);
    }
}