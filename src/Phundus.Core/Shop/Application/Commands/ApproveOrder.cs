namespace Phundus.Core.Shop.Application.Commands
{
    using Cqrs;
    using Domain.Model.Ordering;
    using IdentityAndAccess.Queries;

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

            MemberInRole.ActiveChief(order.Organization.Id, command.InitiatorId);

            order.Approve(command.InitiatorId);
        }
    }
}