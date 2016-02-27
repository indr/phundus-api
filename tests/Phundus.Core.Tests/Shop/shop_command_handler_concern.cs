namespace Phundus.Tests.Shop
{
    using Machine.Specifications;
    using Phundus.Cqrs;
    using Phundus.Shop.Model;

    public abstract class shop_command_handler_concern<TCommand, THandler> : command_handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static shop_factory make;

        protected static IUserInRole userInRole;

        private Establish ctx = () =>
        {
            make = new shop_factory(fake);

            userInRole = depends.on<IUserInRole>();
        };
    }
}