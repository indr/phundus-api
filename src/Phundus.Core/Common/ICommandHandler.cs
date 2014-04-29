namespace Phundus.Core.Common
{
    public interface ICommandHandler<T> : ICommandHandler where T : ICommand
    {
        T Command { get; set; }
    }

    public interface ICommandHandler
    {
        void Execute();
    }
}