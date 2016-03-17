﻿namespace Phundus.Shop.Application
{
    using System;
    using Authorization;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Collaborators;

    // TODO: Rename to AddCartItem
    public class AddArticleToCart : ICommand
    {
        public AddArticleToCart(InitiatorId initiatorId, CartItemId cartItemId, LessorId lessorId, ArticleId articleId,
            DateTime fromUtc,
            DateTime toUtc, int quantity)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (cartItemId == null) throw new ArgumentNullException("cartItemId");
            if (lessorId == null) throw new ArgumentNullException("lessorId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            InitiatorId = initiatorId;
            CartItemId = cartItemId;
            LessorId = lessorId;
            ArticleId = articleId;
            // TODO: Change to LesseeId
            UserId = new UserId(initiatorId.Id);
            FromUtc = fromUtc;
            ToUtc = toUtc;
            Quantity = quantity;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public CartItemId CartItemId { get; protected set; }
        public LessorId LessorId { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
        public UserId UserId { get; protected set; }
        public DateTime FromUtc { get; protected set; }
        public DateTime ToUtc { get; protected set; }
        public int Quantity { get; protected set; }
    }

    public class AddArticleToCartHandler : IHandleCommand<AddArticleToCart>
    {
        private readonly IArticleService _articleService;
        private readonly IAuthorize _authorize;
        private readonly ICartRepository _cartRepository;
        private readonly ICollaboratorService _collaboratorService;
        private readonly ILesseeService _lesseeService;

        public AddArticleToCartHandler(IAuthorize authorize, ICollaboratorService collaboratorService,
            ICartRepository cartRepository, ILesseeService lesseeService, IArticleService articleService)
        {
            _authorize = authorize;
            _collaboratorService = collaboratorService;
            _cartRepository = cartRepository;
            _lesseeService = lesseeService;
            _articleService = articleService;
        }

        [Transaction]
        public void Handle(AddArticleToCart command)
        {
            var lessee = _lesseeService.GetById(new LesseeId(command.InitiatorId));
            var article = _articleService.GetById(command.LessorId, command.ArticleId, lessee.LesseeId);

            var cart = GetCart(command);

            cart.AddItem(command.CartItemId, article, command.FromUtc, command.ToUtc, command.Quantity);
        }

        private Cart GetCart(AddArticleToCart command)
        {
            var cart = _cartRepository.FindByUserGuid(command.UserId);
            if (cart == null)
            {
                cart = new Cart(command.UserId);
                _cartRepository.Add(cart);
            }
            return cart;
        }
    }
}