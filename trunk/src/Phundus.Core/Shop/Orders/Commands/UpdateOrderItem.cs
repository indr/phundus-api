namespace Phundus.Core.Shop.Orders.Commands
{
    using System;
    using Cqrs;
    using IdentityAndAccess.Queries;
    using Repositories;

    public class UpdateOrderItem
    {
        public int OrderId { get; set; }
        public Guid OrderItemId { get; set; }
        public int InitiatorId { get; set; }

        public int Amount { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class UpdateOrderItemHandler : IHandleCommand<UpdateOrderItem>
    {
        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(UpdateOrderItem command)
        {
            var order = OrderRepository.GetById(command.OrderId);

            MemberInRole.ActiveChief(order.OrganizationId, command.InitiatorId);

            order.ChangeAmount(command.OrderItemId, command.Amount);
            order.ChangeItemPeriod(command.OrderItemId, command.From, command.To);
        }
    }
}