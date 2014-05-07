namespace Phundus.Core.Cqrs
{
    public interface ICommandDispatcher
    {
        void Dispatch<TCommand>(TCommand command);
    }
}