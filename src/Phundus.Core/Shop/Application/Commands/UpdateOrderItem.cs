namespace Phundus.Core.Shop.Application.Commands
{
    using System;
    using Castle.Transactions;
    using Common;
    using Common.Cqrs;
    using Common.Domain.Model;
    using Common.Extensions;
    using Cqrs;
    using Domain.Model.Ordering;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using IdentityAndAccess.Queries;

    public class UpdateOrderItem : ICommand
    {
        public UpdateOrderItem(UserId initiatorId, OrganizationId organizationId, OrderId orderId, OrderItemId orderItemId, Period newPeriod, int newQuantity)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "Initiator id must be provided.");
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderId, "Order id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderItemId, "Order item id must be provided.");
            AssertionConcern.AssertArgumentNotNull(newPeriod, "New period must be provided.");

            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            OrderId = orderId;
            OrderItemId = orderItemId;
            FromUtc = newPeriod.FromUtc;
            ToUtc = newPeriod.ToUtc;
            Quantity = newQuantity;
        }

        public UserId InitiatorId { get; private set; }
        public OrganizationId OrganizationId { get; private set; }
        public OrderId OrderId { get; private set; }
        public OrderItemId OrderItemId { get; private set; }
        public DateTime FromUtc { get; private set; }
        public DateTime ToUtc { get; private set; }
        public int Quantity { get; private set; }
    }

    public class UpdateOrderItemHandler : IHandleCommand<UpdateOrderItem>
    {
        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        [Transaction]
        public void Handle(UpdateOrderItem command)
        {
            var order = OrderRepository.GetById(command.OrderId.Id);

            MemberInRole.ActiveChief(order.Organization.Id, command.InitiatorId.Id);

            order.ChangeQuantity(command.InitiatorId, command.OrderItemId.Id, command.Quantity);
            order.ChangeItemPeriod(command.InitiatorId, command.OrderItemId.Id,
                command.FromUtc.ToLocalStartOfTheDayInUtc(),
                command.ToUtc.ToLocalEndOfTheDayInUtc());
        }
    }
}