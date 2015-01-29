namespace Phundus.Core.Shop.Application.Commands
{
    using Castle.Transactions;
    using Common.Cqrs;
    using Cqrs;
    using Ddd;
    using Domain.Model.Identity;
    using Domain.Model.Ordering;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using IdentityAndAccess.Queries;
    using Orders.Model;
    using Orders.Services;

    public class CreateEmptyOrder : ICommand
    {
        public CreateEmptyOrder(UserId initiatorId, OrganizationId organizationId, UserId customerId)
        {
            InitiatorId = initiatorId.Id;
            OrganizationId = organizationId.Id;
            UserId = customerId.Id;            
        }

        public int OrganizationId { get; private set; }
        public int InitiatorId { get; private set; }
        public int UserId { get; private set; }

        public int ResultingOrderId { get; set; }
    }

    public class CreateEmptyOrderHandler : IHandleCommand<CreateEmptyOrder>
    {
        public IMemberInRole MemberInRole { get; set; }

        public IOrderRepository Repository { get; set; }

        public IOrganizationService OrganizationService { get; set; }

        public IBorrowerService BorrowerService { get; set; }

        [Transaction]
        public void Handle(CreateEmptyOrder command)
        {
            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            var order = new Order(
                OrganizationService.ById(command.OrganizationId),
                BorrowerService.ById(command.UserId));

            var orderId = Repository.Add(order);

            command.ResultingOrderId = orderId;

            EventPublisher.Publish(new OrderCreated(new OrderId(orderId)));
        }
    }
}