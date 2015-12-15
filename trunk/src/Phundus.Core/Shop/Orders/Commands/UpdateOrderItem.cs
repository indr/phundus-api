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
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }
        public decimal ItemTotal { get; set; }
    }

    public class UpdateOrderItemHandler : IHandleCommand<UpdateOrderItem>
    {
        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(UpdateOrderItem command)
        {
            var order = OrderRepository.GetById(command.OrderId);

            MemberInRole.ActiveChief(order.Organization.Id, command.InitiatorId);

            order.ChangeAmount(command.OrderItemId, command.Amount);
            order.ChangeItemPeriod(command.OrderItemId, command.FromUtc.ToLocalTime().Date.ToUniversalTime(),
                command.ToUtc.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime());

            order.ChangeItemTotal(command.OrderItemId, command.ItemTotal);
        }
    }
}