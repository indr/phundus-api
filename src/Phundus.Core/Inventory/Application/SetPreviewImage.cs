﻿namespace Phundus.Inventory.Application
{
    using System;
    using Authorization;
    using Common.Commanding;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model.Articles;
    using Phundus.Authorization;

    public class SetPreviewImage : ICommand
    {
        public SetPreviewImage(InitiatorId initiatorId, ArticleId articleId, string fileName)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (fileName == null) throw new ArgumentNullException("fileName");

            InitiatorId = initiatorId;
            ArticleId = articleId;
            FileName = fileName;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
        public string FileName { get; protected set; }
    }

    public class SetPreviewImageHandler : IHandleCommand<SetPreviewImage>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IAuthorize _authorize;
        private readonly IInitiatorService _initiatorService;

        public SetPreviewImageHandler(IAuthorize authorize, IInitiatorService initiatorService,
            IArticleRepository articleRepository)
        {
            if (authorize == null) throw new ArgumentNullException("authorize");
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            if (articleRepository == null) throw new ArgumentNullException("articleRepository");
            _authorize = authorize;
            _initiatorService = initiatorService;
            _articleRepository = articleRepository;
        }

        public void Handle(SetPreviewImage command)
        {
            var initiator = _initiatorService.GetById(command.InitiatorId);
            var article = _articleRepository.GetById(command.ArticleId);

            _authorize.Enforce(initiator.InitiatorId, Manage.Articles(article.Owner.OwnerId));

            article.SetPreviewImage(initiator, command.FileName);
        }
    }
}