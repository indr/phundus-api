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
    using Model.Collaborators;
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
        private readonly ICollaboratorService _collaboratorService;

        public DeleteArticleHandler(IAuthorize authorize, ICollaboratorService collaboratorService,
            IArticleRepository articleRepository)
        {            
            _authorize = authorize;
            _collaboratorService = collaboratorService;
            _articleRepository = articleRepository;
        }

        [Transaction]
        public void Handle(DeleteArticle command)
        {
            var initiator = _collaboratorService.Initiator(command.InitiatorId);
            var article = _articleRepository.GetById(command.ArticleId);

            _authorize.Enforce(initiator.InitiatorId, Manage.Articles(article.Owner.OwnerId));

            _articleRepository.Remove(article);

            // Pass manager
            EventPublisher.Publish(new ArticleDeleted(initiator, article.ArticleShortId, article.ArticleId,
                article.Owner.OwnerId));
        }
    }
}