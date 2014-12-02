namespace Phundus.Core.Shop.Application.Commands
{
    using System;
    using Cqrs;
    using Domain.Model.Ordering;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using IdentityAndAccess.Queries;
    using Inventory.Domain.Model.Catalog;

    public class AddOrderItem
    {
        public UserId InitiatorId { get; set; }
        public OrganizationId OrganizationId { get; set; }
        public OrderId OrderId { get; set; }
        
        public ArticleId ArticleId { get; set; }
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }
        public int Quantity { get; set; }

        public Guid ResultingOrderItemId { get; set; }
    }

    public class AddOrderItemHandler : IHandleCommand<AddOrderItem>
    {
        // TODO: Use integration service
        public IArticleRepository ArticleRepository { get; set; }

        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(AddOrderItem command)
        {
            var order = OrderRepository.GetById(command.OrderId.Id);

            var article = ArticleRepository.GetById(order.Organization.Id, command.ArticleId.Id);
            
            MemberInRole.ActiveChief(order.Organization.Id, command.InitiatorId.Id);

            var toUtc = command.ToUtc.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime();
            var item = order.AddItem(command.InitiatorId, article, command.FromUtc.ToLocalTime().Date.ToUniversalTime(),
                toUtc, command.Quantity);

            command.ResultingOrderItemId = item.Id;
        }
    }
}