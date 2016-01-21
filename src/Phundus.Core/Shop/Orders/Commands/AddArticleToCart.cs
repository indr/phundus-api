namespace Phundus.Shop.Orders.Commands
{
    using System;
    using Authorization;
    using Common.Domain.Model;
    using Cqrs;
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
        private readonly IArticleService _articleService;
        private readonly IAuthorizationDispatcher _authorizationDispatcher;
        private readonly ICartRepository _cartRepository;

        public AddArticleToCartHandler(IAuthorizationDispatcher authorizationDispatcher,
            ICartRepository cartRepository, IArticleService articleService)
        {
            if (authorizationDispatcher == null) throw new ArgumentNullException("authorizationDispatcher");
            if (cartRepository == null) throw new ArgumentNullException("cartRepository");
            if (articleService == null) throw new ArgumentNullException("articleService");
            _authorizationDispatcher = authorizationDispatcher;
            _cartRepository = cartRepository;
            _articleService = articleService;
        }

        public void Handle(AddArticleToCart command)
        {
            _authorizationDispatcher.Dispatch(new RentArticle(new ArticleId(1)));
            //_authorize.User(command.InitiatorId, Rent.Article(command.ArticleId));

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