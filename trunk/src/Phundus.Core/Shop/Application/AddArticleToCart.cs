namespace Phundus.Shop.Application
{
    using System;
    using Authorization;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model;
    using Phundus.Authorization;

    public class AddArticleToCart : ICommand
    {
        public AddArticleToCart(InitiatorId initiatorId, CartItemId cartItemId, ArticleId articleId, DateTime fromUtc,
            DateTime toUtc, int quantity)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (cartItemId == null) throw new ArgumentNullException("cartItemId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            InitiatorId = initiatorId;
            CartItemId = cartItemId;
            ArticleId = articleId;
            UserId = new UserId(initiatorId.Id);            
            FromUtc = fromUtc;
            ToUtc = toUtc;
            Quantity = quantity;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public CartItemId CartItemId { get; protected set; }
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

        [Transaction]
        public void Handle(AddArticleToCart command)
        {
            var initiator = _initiatorService.GetById(command.InitiatorId);
            var article = _articleService.GetById(command.ArticleId, command.UserId);

            _authorize.Enforce(initiator.InitiatorId, Rent.Article(article));

            var cart = GetCart(command);

            cart.AddItem(article, command.FromUtc, command.ToUtc, command.Quantity);
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