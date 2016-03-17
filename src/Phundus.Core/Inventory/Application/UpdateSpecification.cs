namespace Phundus.Inventory.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Articles;
    using Model.Collaborators;

    public class UpdateSpecification : ICommand
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

        public InitiatorId InitiatorId { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
        public string Specification { get; protected set; }
    }

    public class UpdateSpecificationHandler : IHandleCommand<UpdateSpecification>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICollaboratorService _collaboratorService;

        public UpdateSpecificationHandler(ICollaboratorService collaboratorService, IArticleRepository articleRepository)
        {
            _collaboratorService = collaboratorService;
            _articleRepository = articleRepository;
        }

        [Transaction]
        public void Handle(UpdateSpecification command)
        {
            var article = _articleRepository.GetById(command.ArticleId);
            var initiator = _collaboratorService.Manager(command.InitiatorId, article.OwnerId);

            article.ChangeSpecification(initiator, command.Specification);
        }
    }
}