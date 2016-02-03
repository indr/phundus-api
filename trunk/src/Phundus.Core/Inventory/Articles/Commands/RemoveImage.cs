﻿namespace Phundus.Inventory.Articles.Commands
{
    using System;
    using System.IO;
    using Authorization;
    using Common.Domain.Model;
    using Cqrs;
    using Integration.IdentityAccess;
    using Phundus.Authorization;
    using Repositories;

    public class RemoveImage
    {
        public RemoveImage(InitiatorId initiatorId, ArticleId articleId, string fileName)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (fileName == null) throw new ArgumentNullException("fileName");

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException(
                    String.Format(
                        @"The file name ""{0}"" contains invalid characters. Did you mistakenly provide path information?",
                        fileName), "fileName");

            InitiatorId = initiatorId;
            ArticleId = articleId;
            FileName = fileName;
        }

        public InitiatorId InitiatorId { get; set; }
        public ArticleId ArticleId { get; set; }
        public string FileName { get; set; }
    }

    public class RemoveImageHandler : IHandleCommand<RemoveImage>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IAuthorize _authorize;
        private readonly IInitiatorService _initiatorService;

        public RemoveImageHandler(IAuthorize authorize, IInitiatorService initiatorService,
            IArticleRepository articleRepository)
        {
            if (authorize == null) throw new ArgumentNullException("authorize");
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            if (articleRepository == null) throw new ArgumentNullException("articleRepository");
            _authorize = authorize;
            _initiatorService = initiatorService;
            _articleRepository = articleRepository;
        }

        public void Handle(RemoveImage command)
        {
            var initiator = _initiatorService.GetActiveById(command.InitiatorId);
            var article = _articleRepository.GetById(command.ArticleId.Id);

            _authorize.Enforce(initiator.InitiatorId, Manage.Articles(article.Owner.OwnerId));

            article.RemoveImage(initiator, command.FileName);
        }
    }
}