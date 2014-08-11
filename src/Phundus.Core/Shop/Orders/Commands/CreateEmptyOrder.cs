namespace Phundus.Core.Shop.Orders.Commands
{
    using System;
    using Contracts.Services;
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Queries;
    using IdentityAndAccess.Users.Repositories;
    using Model;
    using Repositories;

    public class CreateEmptyOrder
    {
        public int OrganizationId { get; set; }
        public int InitiatorId { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
    }

    public class CreateEmptyOrderHandler : IHandleCommand<CreateEmptyOrder>
    {
        public IMemberInRole MemberInRole { get; set; }

        public IOrderRepository Repository { get; set; }

        public IBorrowerService Borrower { get; set; }

        public IUserRepository UserRepository { get; set; }

        public void Handle(CreateEmptyOrder command)
        {
            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            // TODO: Via BorrowerService
            var order = new Order(
                command.OrganizationId,
                UserRepository.ById(command.UserId));

            var orderId = Repository.Add(order);

            command.OrderId = orderId;

            EventPublisher.Publish(new OrderCreated());
        }
    }
}