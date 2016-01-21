namespace Phundus.Shop.Orders.Commands
{
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Queries;
    using Repositories;

    public class ApproveOrder
    {
        public UserId InitiatorId { get; set; }
        public int OrderId { get; set; }
    }

    public class ApproveOrderHandler : IHandleCommand<ApproveOrder>
    {
        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(ApproveOrder command)
        {
            var order = OrderRepository.GetById(command.OrderId);

            MemberInRole.ActiveManager(order.Lessor.LessorId.Id, command.InitiatorId);

            order.Approve(command.InitiatorId);
        }
    }
}