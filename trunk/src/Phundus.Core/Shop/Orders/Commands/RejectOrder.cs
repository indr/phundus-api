namespace Phundus.Core.Shop.Orders.Commands
{
    using Cqrs;
    using IdentityAndAccess.Queries;
    using Repositories;

    public class RejectOrder
    {
        public int InitiatorId { get; set; }
        public int OrderId { get; set; }
    }

    public class RejectOrderHandler : IHandleCommand<RejectOrder>
    {
        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(RejectOrder command)
        {
            var order = OrderRepository.GetById(command.OrderId);

            MemberInRole.ActiveChief(order.OrganizationId, command.InitiatorId);

            order.Reject();
        }
    }
}