namespace Phundus.Inventory.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Articles;
    using Model.Collaborators;

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
        private readonly ICollaboratorService _collaboratorService;

        public ChangePricesHandler(ICollaboratorService collaboratorService,
            IArticleRepository articleRepository)
        {
            _collaboratorService = collaboratorService;
            _articleRepository = articleRepository;
        }

        [Transaction]
        public void Handle(ChangePrices command)
        {
            var article = _articleRepository.GetById(command.ArticleId);
            var manager = _collaboratorService.Manager(command.InitiatorId, article.OwnerId);

            article.ChangePrices(manager, command.PublicPrice, command.MemberPrice);
        }
    }
}