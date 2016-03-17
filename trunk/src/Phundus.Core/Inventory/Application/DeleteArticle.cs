namespace Phundus.Inventory.Application
{
    using System;
    using Articles.Model;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Common.Eventing;
    using Model.Articles;
    using Model.Collaborators;

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
        private readonly ICollaboratorService _collaboratorService;

        public DeleteArticleHandler(ICollaboratorService collaboratorService, IArticleRepository articleRepository)
        {
            _collaboratorService = collaboratorService;
            _articleRepository = articleRepository;
        }

        [Transaction]
        public void Handle(DeleteArticle command)
        {
            var article = _articleRepository.GetById(command.ArticleId);
            var manager = _collaboratorService.Manager(command.InitiatorId, article.OwnerId);

            _articleRepository.Remove(article);
            
            EventPublisher.Publish(new ArticleDeleted(manager, article.ArticleShortId, article.ArticleId,
                article.Owner.OwnerId));
        }
    }
}