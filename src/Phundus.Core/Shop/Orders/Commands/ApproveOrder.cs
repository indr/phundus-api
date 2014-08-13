namespace Phundus.Core.Shop.Orders.Commands
{
    using Cqrs;
    using IdentityAndAccess.Queries;
    using Repositories;

    public class ApproveOrder
    {
        public int InitiatorId { get; set; }
        public int OrderId { get; set; }
    }

    public class ApproveOrderHandler : IHandleCommand<ApproveOrder>
    {
        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(ApproveOrder command)
        {
            var order = OrderRepository.GetById(command.OrderId);

            MemberInRole.ActiveChief(order.OrganizationId, command.InitiatorId);

            order.Approve();
        }
    }
}