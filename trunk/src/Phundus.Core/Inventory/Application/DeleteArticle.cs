namespace Phundus.Inventory.Application
{
    using System;
    using Articles.Model;
    using Authorization;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Common.Eventing;
    using Integration.IdentityAccess;
    using Model.Articles;
    using Phundus.Authorization;

    public class DeleteArticle : ICommand
    {
        public DeleteArticle(InitiatorId initiatorId, ArticleId articleId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            InitiatorId = initiatorId;
            ArticleId = articleId;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
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

        [Transaction]
        public void Handle(DeleteArticle command)
        {
            var initiator = _initiatorService.GetById(command.InitiatorId);
            var article = _articleRepository.GetById(command.ArticleId);

            _authorize.Enforce(initiator.InitiatorId, Manage.Articles(article.Owner.OwnerId));

            _articleRepository.Remove(article);

            EventPublisher.Publish(new ArticleDeleted(initiator, article.ArticleShortId, article.ArticleId,
                article.Owner.OwnerId));
        }
    }
}