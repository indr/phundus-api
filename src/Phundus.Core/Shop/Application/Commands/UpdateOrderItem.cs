namespace Phundus.Core.Shop.Application.Commands
{
    using System;
    using Common.Extensions;
    using Cqrs;
    using Domain.Model.Ordering;
    using IdentityAndAccess.Domain.Model.Users;
    using IdentityAndAccess.Queries;

    public class UpdateOrderItem
    {
        public UserId InitiatorId { get; set; }

        public int OrderId { get; set; }
        public Guid OrderItemId { get; set; }

        public int Quantity { get; set; }
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }
    }

    public class UpdateOrderItemHandler : IHandleCommand<UpdateOrderItem>
    {
        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(UpdateOrderItem command)
        {
            var order = OrderRepository.GetById(command.OrderId);

            MemberInRole.ActiveChief(order.Organization.Id, command.InitiatorId.Id);

            order.ChangeQuantity(command.InitiatorId, command.OrderItemId, command.Quantity);
            order.ChangeItemPeriod(command.InitiatorId, command.OrderItemId,
                command.FromUtc.ToLocalStartOfTheDayInUtc(),
                command.ToUtc.ToLocalEndOfTheDayInUtc());
        }
    }
}