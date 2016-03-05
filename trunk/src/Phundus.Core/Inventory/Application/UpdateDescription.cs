namespace Phundus.Inventory.Application
{
    using System;
    using Authorization;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model.Articles;
    using Phundus.Authorization;

    public class UpdateDescription: ICommand
    {
        public UpdateDescription(InitiatorId initiatorId, ArticleId articleId, string description)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (description == null) throw new ArgumentNullException("description");
            InitiatorId = initiatorId;
            ArticleId = articleId;
            Description = description;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
        public string Description { get; protected set; }
    }

    public class UpdateDescriptionHandler : IHandleCommand<UpdateDescription>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IAuthorize _authorize;
        private readonly IInitiatorService _initiatorService;

        public UpdateDescriptionHandler(IAuthorize authorize, IInitiatorService initiatorService,
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
        public void Handle(UpdateDescription command)
        {
            var initiator = _initiatorService.GetById(command.InitiatorId);
            var article = _articleRepository.GetById(command.ArticleId);

            _authorize.Enforce(initiator.InitiatorId, Manage.Articles(article.Owner.OwnerId));

            article.ChangeDescription(initiator, command.Description);
        }
    }
}