namespace Phundus.Infrastructure.Cqrs
{
    public interface IHandleCommand
    {
    }

    public interface IHandleCommand<in TCommand> : IHandleCommand
    {
        void Handle(TCommand command);
    }
}