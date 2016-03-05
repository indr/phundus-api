namespace Phundus.Tests.Shop
{
    using Common.Commanding;
    using Machine.Specifications;
    using Phundus.Shop.Model;

    public abstract class shop_command_handler_concern<TCommand, THandler> : command_handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand> where TCommand : ICommand
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