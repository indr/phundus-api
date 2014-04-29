namespace Phundus.Core.Common
{
    public interface ICommandDispatcher
    {
        void Dispatch(ICommand command);
    }
}