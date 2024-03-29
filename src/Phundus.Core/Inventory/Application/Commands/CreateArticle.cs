﻿namespace Phundus.Inventory.Application
{
    using System;
    using Articles.Model;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Common.Eventing;
    using Model.Articles;
    using Model.Collaborators;
    using Model.Stores;

    public class CreateArticle : ICommand
    {
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
        private readonly ICollaboratorService _collaboratorService;
        private readonly IOwnerService _ownerService;
        private readonly IStoreRepository _storeRepository;

        public CreateArticleHandler(ICollaboratorService collaboratorService, IArticleRepository articleRepository,
            IStoreRepository storeRepository, IOwnerService ownerService)
        {
            _collaboratorService = collaboratorService;
            _articleRepository = articleRepository;
            _storeRepository = storeRepository;
            _ownerService = ownerService;
        }

        [Transaction]
        public void Handle(CreateArticle command)
        {
            var owner = _ownerService.GetById(command.OwnerId);
            var manager = _collaboratorService.Manager(command.InitiatorId, owner.OwnerId);

            var store = _storeRepository.GetById(command.StoreId);
            if (!Equals(store.OwnerId, command.OwnerId))
                throw new Exception("The store does not belong to the owner specified.");

            // TODO: 
            var article = new Article(owner, store.StoreId, command.ArticleId, command.ArticleShortId, command.Name,
                command.GrossStock, command.PublicPrice, command.MemberPrice);

            _articleRepository.Add(article);

            EventPublisher.Publish(new ArticleCreated(manager, article.Owner, article.StoreId, store.Name,
                article.ArticleShortId, article.ArticleId,
                article.Name, article.GrossStock, article.PublicPrice, article.MemberPrice));
        }
    }
}