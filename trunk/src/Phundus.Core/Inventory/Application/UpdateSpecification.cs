namespace Phundus.Inventory.Application
{
    using System;
    using Authorization;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Articles;
    using Model.Collaborators;
    using Phundus.Authorization;

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
        private readonly IAuthorize _authorize;
        private readonly ICollaboratorService _collaboratorService;

        public UpdateSpecificationHandler(IAuthorize authorize, ICollaboratorService collaboratorService,
            IArticleRepository articleRepository)
        {
            _authorize = authorize;
            _collaboratorService = collaboratorService;
            _articleRepository = articleRepository;
        }

        [Transaction]
        public void Handle(UpdateSpecification command)
        {
            var initiator = _collaboratorService.Initiator(command.InitiatorId);
            var article = _articleRepository.GetById(command.ArticleId);

            _authorize.Enforce(initiator.InitiatorId, Manage.Articles(article.Owner.OwnerId));

            article.ChangeSpecification(initiator, command.Specification);
        }
    }
}