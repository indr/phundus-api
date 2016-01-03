namespace Phundus.Core.Shop.Orders.Commands
{
    using System;
    using System.Security;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAndAccess.Queries;
    using Repositories;
    using Shop.Services;

    public class AddArticleToCart
    {
        public int CartId { get; set; }
        public UserId InitiatorId { get; set; }

        public int ArticleId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }

    public class AddArticleToCartHandler : IHandleCommand<AddArticleToCart>
    {
        public ICartRepository CartRepository { get; set; }

        public IArticleService ArticleService { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(AddArticleToCart command)
        {
            var cart = CartRepository.FindById(command.CartId);
            if (cart == null)
                throw new CartNotFoundException();

            if (cart.CustomerId != command.InitiatorId.Id)
                throw new SecurityException();

            var article = ArticleService.GetById(command.ArticleId);

            if (!MemberInRole.IsActiveMember(article.Owner.OwnerId, command.InitiatorId))
                throw new SecurityException();

            cart.AddItem(article, command.Quantity,
                command.DateFrom, command.DateTo);
        }
    }
}