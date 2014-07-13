namespace Phundus.Core.Cqrs
{
    using System.Collections;

    public interface IHandleCommand
    {
    }

    public interface IHandleCommand<in TCommand> : IHandleCommand
    {
        void Handle(TCommand command);
    }
}