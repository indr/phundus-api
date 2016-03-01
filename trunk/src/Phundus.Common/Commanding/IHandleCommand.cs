namespace Phundus.Common.Commanding
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