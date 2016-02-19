namespace Phundus.Tests.Shop.Orders.Commands
{
    using Machine.Specifications;
    using Phundus.Cqrs;

    public abstract class shop_command_handler_concern<TCommand, THandler> : command_handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static shop_factory make;

        private Establish ctx = () => { make = new shop_factory(fake); };
    }
}