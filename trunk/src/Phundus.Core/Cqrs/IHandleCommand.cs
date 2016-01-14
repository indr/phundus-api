namespace Phundus.Cqrs
{
    using Castle.Transactions;

    public interface IHandleCommand
    {
    }

    public interface IHandleCommand<in TCommand> : IHandleCommand
    {
        [Transaction]
        void Handle(TCommand command);
    }
}