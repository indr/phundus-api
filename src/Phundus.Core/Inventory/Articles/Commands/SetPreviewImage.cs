namespace Phundus.Inventory.Articles.Commands
{
    using System;
    using Authorization;
    using Common.Domain.Model;
    using Cqrs;
    using Integration.IdentityAccess;
    using Phundus.Authorization;
    using Repositories;

    public class SetPreviewImage : ICommand
    {
        public SetPreviewImage(InitiatorId initiatorId, ArticleShortId articleShortId, string fileName)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (fileName == null) throw new ArgumentNullException("fileName");
            InitiatorId = initiatorId;
            ArticleShortId = articleShortId;
            FileName = fileName;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public ArticleShortId ArticleShortId { get; protected set; }
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
            var initiator = _initiatorService.GetActiveById(command.InitiatorId);
            var article = _articleRepository.GetById(command.ArticleShortId);

            _authorize.Enforce(initiator.InitiatorId, Manage.Articles(article.Owner.OwnerId));

            article.SetPreviewImage(initiator, command.FileName);
        }
    }
}