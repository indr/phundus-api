namespace Phundus.Core.Cqrs
{
    using Common.Cqrs;

    public interface ICommandDispatcher
    {
        void Dispatch<TCommand>(TCommand command) where TCommand : ICommand;
    }
}