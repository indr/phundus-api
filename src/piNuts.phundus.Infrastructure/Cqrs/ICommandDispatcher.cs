namespace Phundus.Infrastructure.Cqrs
{
    public interface ICommandDispatcher
    {
        void Dispatch<TCommand>(TCommand command);
    }
}