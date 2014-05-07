namespace Phundus.Core.Cqrs
{
    public interface IHandleCommand
    {
    }

    public interface IHandleCommand<in TCommand> : IHandleCommand
    {
        void Handle(TCommand command);
    }

    //public interface IHandleCommand<TCommand, TAggregate>
    //{
    //   IEnumerable Handle(Func<Guid, TAggregate> al, TCommand c);
    //}
}