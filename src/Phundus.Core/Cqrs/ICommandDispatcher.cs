namespace Phundus.Core.Cqrs
{
    public interface ICommandDispatcher
    {
        void Dispatch(ICommand command);
    }
}