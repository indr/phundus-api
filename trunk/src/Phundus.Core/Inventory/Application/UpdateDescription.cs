namespace Phundus.Inventory.Application
{
    using System;
    using Authorization;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model.Articles;
    using Model.Collaborators;

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
        private readonly ICollaboratorService _collaboratorService;

        public UpdateDescriptionHandler(ICollaboratorService collaboratorService, IArticleRepository articleRepository)
        {
            _collaboratorService = collaboratorService;
            _articleRepository = articleRepository;
        }
        
        [Transaction]
        public void Handle(UpdateDescription command)
        {            
            var article = _articleRepository.GetById(command.ArticleId);
            var manager = _collaboratorService.Manager(command.InitiatorId, article.OwnerId);            

            article.ChangeDescription(manager, command.Description);
        }
    }
}