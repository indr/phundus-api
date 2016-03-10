namespace Phundus.Common.Commanding
{
    public interface ICommandDispatcher
    {
        void Dispatch<TCommand>(TCommand command);
    }
}