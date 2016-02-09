namespace Phundus.Inventory.Articles.Commands
{
    using System;
    using Authorization;
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;
    using Integration.IdentityAccess;
    using Model;
    using Phundus.Authorization;
    using Repositories;

    public class DeleteArticle
    {
        public DeleteArticle(InitiatorId initiatorId, int articleId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            InitiatorId = initiatorId;
            ArticleId = articleId;
        }

        public InitiatorId InitiatorId { get; set; }
        public int ArticleId { get; set; }
    }

    public class DeleteArticleHandler : IHandleCommand<DeleteArticle>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IAuthorize _authorize;
        private readonly IInitiatorService _initiatorService;

        public DeleteArticleHandler(IAuthorize authorize, IInitiatorService initiatorService,
            IArticleRepository articleRepository)
        {
            if (authorize == null) throw new ArgumentNullException("authorize");
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            if (articleRepository == null) throw new ArgumentNullException("articleRepository");
            _authorize = authorize;
            _initiatorService = initiatorService;
            _articleRepository = articleRepository;
        }

        public void Handle(DeleteArticle command)
        {
            var initiator = _initiatorService.GetActiveById(command.InitiatorId);
            var article = _articleRepository.GetById(command.ArticleId);

            _authorize.Enforce(initiator.InitiatorId, Manage.Articles(article.Owner.OwnerId));

            _articleRepository.Remove(article);

            EventPublisher.Publish(new ArticleDeleted(initiator, article.ArticleShortId, article.ArticleId,
                article.Owner.OwnerId));
        }
    }
}