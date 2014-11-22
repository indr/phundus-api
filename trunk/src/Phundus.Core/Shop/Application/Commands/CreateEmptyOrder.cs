namespace Phundus.Core.Shop.Application.Commands
{
    using Cqrs;
    using Ddd;
    using Domain.Model.Identity;
    using Domain.Model.Ordering;
    using IdentityAndAccess.Queries;
    using Orders.Model;
    using Orders.Services;

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

        public IOrganizationService OrganizationService { get; set; }

        public IBorrowerService BorrowerService { get; set; }

        public void Handle(CreateEmptyOrder command)
        {
            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            var order = new Order(
                OrganizationService.ById(command.OrganizationId),
                BorrowerService.ById(command.UserId));

            var orderId = Repository.Add(order);

            command.OrderId = orderId;

            EventPublisher.Publish(new OrderCreated {OrderId = orderId});
        }
    }
}