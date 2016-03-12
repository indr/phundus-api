namespace Phundus.Inventory.Application
{
    using System;
    using Articles.Model;
    using Authorization;
    using Castle.Transactions;
    using Common;
    using Common.Commanding;
    using Common.Domain.Model;
    using Common.Eventing;
    using Integration.IdentityAccess;
    using Model;
    using Model.Articles;
    using Model.Collaborators;
    using Phundus.Authorization;
    using Stores.Repositories;

    public class CreateArticle : ICommand
    {
        public CreateArticle(InitiatorId initiatorId, OwnerId ownerId, StoreId storeId, string name, int grossStock)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "InitiatorId must be provided.");
            AssertionConcern.AssertArgumentNotNull(ownerId, "OwnerId must be provided.");
            AssertionConcern.AssertArgumentNotNull(storeId, "StoreId must be provided.");
            AssertionConcern.AssertArgumentNotNull(name, "Name must be provided.");
            InitiatorId = initiatorId;
            OwnerId = ownerId;
            Name = name;
            GrossStock = grossStock;
        }

        public CreateArticle(InitiatorId initiatorId, OwnerId ownerId, StoreId storeId, ArticleId articleId,
            ArticleShortId articleShortId, string name, int grossStock, decimal publicPrice, decimal? memberPrice)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (name == null) throw new ArgumentNullException("name");
            InitiatorId = initiatorId;
            OwnerId = ownerId;
            StoreId = storeId;
            ArticleId = articleId;
            ArticleShortId = articleShortId;
            Name = name;
            GrossStock = grossStock;
            MemberPrice = memberPrice;
            PublicPrice = publicPrice;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OwnerId OwnerId { get; protected set; }
        public StoreId StoreId { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
        public ArticleShortId ArticleShortId { get; protected set; }
        public string Name { get; protected set; }
        public int GrossStock { get; protected set; }
        public decimal PublicPrice { get; protected set; }
        public decimal? MemberPrice { get; protected set; }
    }

    public class CreateArticleHandler : IHandleCommand<CreateArticle>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IAuthorize _authorize;
        private readonly IInitiatorService _initiatorService;
        private readonly IOwnerService _ownerService;
        private readonly IStoreRepository _storeRepository;

        public CreateArticleHandler(IAuthorize authorize, IInitiatorService initiatorService,
            IArticleRepository articleRepository, IStoreRepository storeRepository,
            IOwnerService ownerService)
        {
            if (authorize == null) throw new ArgumentNullException("authorize");
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            if (articleRepository == null) throw new ArgumentNullException("articleRepository");
            if (storeRepository == null) throw new ArgumentNullException("storeRepository");
            if (ownerService == null) throw new ArgumentNullException("ownerService");
            _authorize = authorize;
            _initiatorService = initiatorService;
            _articleRepository = articleRepository;
            _storeRepository = storeRepository;
            _ownerService = ownerService;
        }

        [Transaction]
        public void Handle(CreateArticle command)
        {
            var initiator = _initiatorService.GetById(command.InitiatorId);
            var owner = _ownerService.GetById(command.OwnerId);

            _authorize.Enforce(initiator.InitiatorId, Create.Article(owner.OwnerId));

            var store = _storeRepository.GetById(command.StoreId);
            if (!Equals(store.Owner.OwnerId, command.OwnerId))
                throw new Exception("The store does not belong to the owner specified.");

            var article = new Article(owner, store.StoreId, command.ArticleId, command.ArticleShortId, command.Name,
                command.GrossStock, command.PublicPrice, command.MemberPrice);

            _articleRepository.Add(article);

            EventPublisher.Publish(new ArticleCreated(initiator, article.Owner, article.StoreId, store.Name,
                article.ArticleShortId, article.ArticleId,
                article.Name, article.GrossStock, article.PublicPrice, article.MemberPrice));
        }
    }
}