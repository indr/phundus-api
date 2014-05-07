namespace Phundus.Core.Cqrs
{
    public interface ICommandHandler<T> : ICommandHandler
    {
        T Command { get; set; }
    }

    public interface ICommandHandler
    {
        void Execute();
    }
}