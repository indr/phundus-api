namespace Phundus.Shop.Orders.Commands
{
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Queries;
    using Repositories;

    public class CloseOrder
    {
        public int OrderId { get; set; }
        public UserId InitiatorId { get; set; }
    }

    public class CloseOrderHandler : IHandleCommand<CloseOrder>
    {
        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(CloseOrder command)
        {
            var order = OrderRepository.GetById(command.OrderId);

            MemberInRole.ActiveManager(order.Lessor.LessorId.Id, command.InitiatorId);

            order.Close(command.InitiatorId);
        }
    }
}