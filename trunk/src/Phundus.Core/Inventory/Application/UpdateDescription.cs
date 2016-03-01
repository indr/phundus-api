﻿namespace Phundus.Inventory.Application
{
    using System;
    using Articles.Repositories;
    using Authorization;
    using Common.Commanding;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Phundus.Authorization;

    public class UpdateDescription
    {
        public UpdateDescription(InitiatorId initiatorId, int articleId, string description)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (description == null) throw new ArgumentNullException("description");
            InitiatorId = initiatorId;
            ArticleId = new ArticleShortId(articleId);
            Description = description;
        }

        public InitiatorId InitiatorId { get; set; }
        public ArticleShortId ArticleId { get; set; }
        public string Description { get; set; }
    }

    public class UpdateArticleDescriptionHandler : IHandleCommand<UpdateDescription>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IAuthorize _authorize;
        private readonly IInitiatorService _initiatorService;

        public UpdateArticleDescriptionHandler(IAuthorize authorize, IInitiatorService initiatorService,
            IArticleRepository articleRepository)
        {
            if (authorize == null) throw new ArgumentNullException("authorize");
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            if (articleRepository == null) throw new ArgumentNullException("articleRepository");
            _authorize = authorize;
            _initiatorService = initiatorService;
            _articleRepository = articleRepository;
        }

        public void Handle(UpdateDescription command)
        {
            var initiator = _initiatorService.GetById(command.InitiatorId);
            var article = _articleRepository.GetById(command.ArticleId);

            _authorize.Enforce(initiator.InitiatorId, Manage.Articles(article.Owner.OwnerId));

            article.ChangeDescription(initiator, command.Description);
        }
    }
}