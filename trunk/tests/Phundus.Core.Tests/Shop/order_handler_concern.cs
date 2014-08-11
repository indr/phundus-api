namespace Phundus.Core.Tests.Shop
{
    using Core.Cqrs;
    using Core.IdentityAndAccess.Queries;
    using Core.IdentityAndAccess.Users.Repositories;
    using Core.Shop.Contracts.Services;
    using Core.Shop.Orders.Repositories;
    using Machine.Specifications;

    public abstract class order_handler_concern<TCommand, THandler> : handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static IMemberInRole memberInRole;

        protected static IOrderRepository repository;

        protected static IBorrowerService borrowerService;

        // TODO: Via BorrowerService
        protected static IUserRepository userRepository;

        protected Establish dependencies = () =>
        {
            memberInRole = depends.on<IMemberInRole>();
            repository = depends.on<IOrderRepository>();
            borrowerService = depends.on<IBorrowerService>();
            userRepository = depends.on<IUserRepository>();
        };
    }
}