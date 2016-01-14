namespace Phundus.Shop.Orders.Commands
{
    using System;
    using Cqrs;
    using IdentityAccess.Queries;
    using Repositories;

    public class RemoveOrderItem
    {
        public int OrderId { get; set; }
        public Guid OrderItemId { get; set; }
        public int InitiatorId { get; set; }
    }

    public class RemoveOrderItemHandler : IHandleCommand<RemoveOrderItem>
    {
        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(RemoveOrderItem command)
        {
            var order = OrderRepository.GetById(command.OrderId);

            MemberInRole.ActiveChief(order.Lessor.LessorId.Id, command.InitiatorId);

            order.RemoveItem(command.OrderItemId);
        }
    }
}