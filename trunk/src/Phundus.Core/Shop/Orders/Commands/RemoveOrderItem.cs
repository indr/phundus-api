namespace Phundus.Core.Shop.Orders.Commands
{
    using System;
    using Cqrs;
    using IdentityAndAccess.Queries;
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
            var order = OrderRepository.ById(command.OrderId);
            if (order == null)
                throw new OrderNotFoundException();

            MemberInRole.ActiveChief(order.Organization.Id, command.InitiatorId);

            order.RemoveItem(command.OrderItemId);
        }
    }
}