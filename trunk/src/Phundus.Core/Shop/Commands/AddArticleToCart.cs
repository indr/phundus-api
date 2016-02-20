namespace Phundus.Shop.Orders.Commands
{
    using System;
    using Authorization;
    using Common.Domain.Model;
    using Cqrs;
    using Integration.IdentityAccess;
    using Model;
    using Phundus.Authorization;
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
            ArticleId = articleId;
            UserId = new UserId(initiatorId.Id);            
            FromUtc = fromUtc;
            ToUtc = toUtc;
            Quantity = quantity;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
        public UserId UserId { get; protected set; }
        public ArticleShortId ArticleShortId { get; protected set; }
        public DateTime FromUtc { get; protected set; }
        public DateTime ToUtc { get; protected set; }
        public int Quantity { get; protected set; }
        public CartItemId ResultingCartItemId { get; set; }
    }

    public class AddArticleToCartHandler : IHandleCommand<AddArticleToCart>
    {
        private readonly IArticleService _articleService;
        private readonly IAuthorize _authorize;
        private readonly IInitiatorService _initiatorService;
        private readonly ICartRepository _cartRepository;

        public AddArticleToCartHandler(IAuthorize authorize, IInitiatorService initiatorService,
            ICartRepository cartRepository, IArticleService articleService)
        {
            if (authorize == null) throw new ArgumentNullException("authorize");
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            if (cartRepository == null) throw new ArgumentNullException("cartRepository");
            if (articleService == null) throw new ArgumentNullException("articleService");
            _authorize = authorize;
            _initiatorService = initiatorService;
            _cartRepository = cartRepository;
            _articleService = articleService;
        }

        public void Handle(AddArticleToCart command)
        {
            var initiator = _initiatorService.GetActiveById(command.InitiatorId);
            var article = _articleService.GetById(command.ArticleId, command.UserId);

            _authorize.Enforce(initiator.InitiatorId, Rent.Article(article));

            var cart = GetCart(command);

            var itemId = cart.AddItem(article, command.FromUtc, command.ToUtc, command.Quantity);

            command.ResultingCartItemId = itemId;
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