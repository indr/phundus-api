﻿namespace Phundus.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Queries;
    using Model;
    using Repositories;
    using Shop.Services;

    public class AddArticleToCart : ICommand
    {
        public AddArticleToCart(InitiatorId initiatorId, ArticleId articleId, DateTime fromUtc,
            DateTime toUtc, int quantity)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            InitiatorId = initiatorId;
            UserGuid = new UserGuid(initiatorId.Id);
            ArticleId = articleId;
            FromUtc = fromUtc;
            ToUtc = toUtc;
            Quantity = quantity;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public UserGuid UserGuid { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
        public DateTime FromUtc { get; protected set; }
        public DateTime ToUtc { get; protected set; }
        public int Quantity { get; protected set; }
        public CartItemGuid ResultingCartItemGuid { get; set; }
    }

    public class AddArticleToCartHandler : IHandleCommand<AddArticleToCart>
    {
        public ICartRepository CartRepository { get; set; }

        public IArticleService ArticleService { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(AddArticleToCart command)
        {
            var article = ArticleService.GetById(command.ArticleId);

            MemberInRole.ActiveMember(article.Owner.OwnerId, command.UserGuid);

            var cart = CartRepository.FindByUserGuid(command.UserGuid);
            if (cart == null)
            {
                cart = new Cart(command.InitiatorId, command.UserGuid);
                CartRepository.Add(cart);
            }
            var itemId = cart.AddItem(article, command.FromUtc, command.ToUtc, command.Quantity);

            command.ResultingCartItemGuid = itemId;
        }
    }
}