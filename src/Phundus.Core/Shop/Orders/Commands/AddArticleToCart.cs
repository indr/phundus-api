namespace Phundus.Shop.Orders.Commands
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
            UserId = new UserId(initiatorId.Id);
            ArticleId = articleId;
            FromUtc = fromUtc;
            ToUtc = toUtc;
            Quantity = quantity;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public UserId UserId { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
        public DateTime FromUtc { get; protected set; }
        public DateTime ToUtc { get; protected set; }
        public int Quantity { get; protected set; }
        public CartItemId ResultingCartItemId { get; set; }
    }

    public class AddArticleToCartHandler : IHandleCommand<AddArticleToCart>
    {
        private readonly IAuthorize _authorize;
        private readonly ICartRepository _cartRepository;
        private readonly IArticleService _articleService;

        public AddArticleToCartHandler(IAuthorize authorize, ICartRepository cartRepository, IArticleService articleService)
        {
            if (authorize == null) throw new ArgumentNullException("authorize");
            if (cartRepository == null) throw new ArgumentNullException("cartRepository");
            if (articleService == null) throw new ArgumentNullException("articleService");
            _authorize = authorize;
            _cartRepository = cartRepository;
            _articleService = articleService;
        }

        public void Handle(AddArticleToCart command)
        {
            _authorize.User(command.InitiatorId, Rent.Article(command.ArticleId));

            var article = _articleService.GetById(command.ArticleId);

            var cart = GetCart(command);

            var itemId = cart.AddItem(article, command.FromUtc, command.ToUtc, command.Quantity);

            command.ResultingCartItemId = itemId;
        }

        private Cart GetCart(AddArticleToCart command)
        {
            var cart = _cartRepository.FindByUserGuid(command.UserId);
            if (cart == null)
            {
                cart = new Cart(command.InitiatorId, command.UserId);
                _cartRepository.Add(cart);
            }
            return cart;
        }
    }

    
}