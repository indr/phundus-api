﻿namespace Phundus.Inventory.Application
{
    using System;
    using Authorization;
    using Common.Commanding;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model.Articles;
    using Phundus.Authorization;

    public class UpdateSpecification
    {
        public UpdateSpecification(InitiatorId initiatorId, ArticleId articleId, string specification)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (specification == null) throw new ArgumentNullException("specification");

            InitiatorId = initiatorId;
            ArticleId = articleId;
            Specification = specification;
        }

        public InitiatorId InitiatorId { get; set; }
        public ArticleId ArticleId { get; set; }
        public string Specification { get; set; }
    }

    public class UpdateArticleSpecificationHandler : IHandleCommand<UpdateSpecification>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IAuthorize _authorize;
        private readonly IInitiatorService _initiatorService;

        public UpdateArticleSpecificationHandler(IAuthorize authorize, IInitiatorService initiatorService,
            IArticleRepository articleRepository)
        {
            if (authorize == null) throw new ArgumentNullException("authorize");
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            if (articleRepository == null) throw new ArgumentNullException("articleRepository");
            _authorize = authorize;
            _initiatorService = initiatorService;
            _articleRepository = articleRepository;
        }

        public void Handle(UpdateSpecification command)
        {
            var initiator = _initiatorService.GetById(command.InitiatorId);
            var article = _articleRepository.GetById(command.ArticleId);

            _authorize.Enforce(initiator.InitiatorId, Manage.Articles(article.Owner.OwnerId));

            article.ChangeSpecification(initiator, command.Specification);
        }
    }
}