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

    public class ChangePrices : ICommand
    {
        public ChangePrices(InitiatorId initiatorId, ArticleId articleId, decimal publicPrice, decimal? memberPrice)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            InitiatorId = initiatorId;
            ArticleId = articleId;
            PublicPrice = publicPrice;
            MemberPrice = memberPrice;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
        public decimal PublicPrice { get; protected set; }
        public decimal? MemberPrice { get; protected set; }
    }

    public class ChangePricesHandler : IHandleCommand<ChangePrices>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IAuthorize _authorize;
        private readonly ICollaboratorService _collaboratorService;

        public ChangePricesHandler(IAuthorize authorize, ICollaboratorService collaboratorService,
            IArticleRepository articleRepository)
        {
            _authorize = authorize;
            _collaboratorService = collaboratorService;
            _articleRepository = articleRepository;
        }

        [Transaction]
        public void Handle(ChangePrices command)
        {
            var initiator = _collaboratorService.Initiator(command.InitiatorId);
            var article = _articleRepository.GetById(command.ArticleId);

            _authorize.Enforce(initiator.InitiatorId, Manage.Articles(article.Owner.OwnerId));

            // TODO: Pass manager
            article.ChangePrices(initiator, command.PublicPrice, command.MemberPrice);
        }
    }
}