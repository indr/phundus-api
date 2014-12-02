namespace Phundus.Core.Shop.Application.Commands
{
    using System;
    using Cqrs;
    using Domain.Model.Ordering;
    using IdentityAndAccess.Domain.Model.Users;
    using IdentityAndAccess.Queries;

    public class UpdateOrderItem
    {
        public UserId InitiatorId { get; set; }

        public int OrderId { get; set; }
        public Guid OrderItemId { get; set; }

        public int Amount { get; set; }
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

            order.ChangeAmount(command.OrderItemId, command.Amount);
            order.ChangeItemPeriod(command.OrderItemId, command.FromUtc.ToLocalTime().Date.ToUniversalTime(),
                command.ToUtc.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime());
        }
    }
}