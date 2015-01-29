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
    using Inventory.Domain.Model.Catalog;

    public class AddOrderItem : ICommand
    {
        public AddOrderItem(UserId initiatorId, OrganizationId organizationId, OrderId orderId, ArticleId articleId, Period period, int quantity)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "Initiator id must be provided.");
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderId, "Order id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(period, "Period must be provided.");
            AssertionConcern.AssertArgumentGreaterThanZero(quantity, "Quantity must be greater than zero.");

            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            OrderId = orderId;
            ArticleId = articleId;
            FromUtc = period.FromUtc;
            ToUtc = period.ToUtc;
            Quantity = quantity;
        }

        public UserId InitiatorId { get; private set; }
        public OrganizationId OrganizationId { get; private set; }
        public OrderId OrderId { get; private set; }

        public ArticleId ArticleId { get; private set; }
        public DateTime FromUtc { get; private set; }
        public DateTime ToUtc { get; private set; }
        public int Quantity { get; private set; }

        public OrderItemId ResultingOrderItemId { get; set; }
    }

    public class AddOrderItemHandler : IHandleCommand<AddOrderItem>
    {
        // TODO: Use integration service
        public IArticleRepository ArticleRepository { get; set; }

        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        [Transaction]
        public void Handle(AddOrderItem command)
        {
            var order = OrderRepository.GetById(command.OrderId.Id);

            var article = ArticleRepository.GetById(order.Organization.Id, command.ArticleId.Id);

            MemberInRole.ActiveChief(order.Organization.Id, command.InitiatorId.Id);

            var item = order.AddItem(command.InitiatorId, article,
                command.FromUtc.ToLocalStartOfTheDayInUtc(),
                command.ToUtc.ToLocalEndOfTheDayInUtc(),
                command.Quantity);

            command.ResultingOrderItemId = new OrderItemId(item.Id);
        }
    }
}