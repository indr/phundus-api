namespace Phundus.Core.Shop.Application.Commands
{
    using System;
    using Cqrs;
    using Domain.Model.Ordering;
    using IdentityAndAccess.Queries;

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
            
            MemberInRole.ActiveChief(order.Organization.Id, command.InitiatorId);

            order.RemoveItem(command.OrderItemId);
        }
    }
}