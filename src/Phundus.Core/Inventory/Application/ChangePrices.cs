namespace Phundus.Inventory.Application
{
    using System;
    using Authorization;
    using Common.Commanding;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model.Articles;
    using Phundus.Authorization;

    public class ChangePrices
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

        public InitiatorId InitiatorId { get; set; }
        public ArticleId ArticleId { get; set; }
        public decimal PublicPrice { get; set; }
        public decimal? MemberPrice { get; set; }
    }

    public class ChangePricesHandler : IHandleCommand<ChangePrices>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IAuthorize _authorize;
        private readonly IInitiatorService _initiatorService;

        public ChangePricesHandler(IAuthorize authorize, IInitiatorService initiatorService,
            IArticleRepository articleRepository)
        {
            if (authorize == null) throw new ArgumentNullException("authorize");
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            if (articleRepository == null) throw new ArgumentNullException("articleRepository");
            _authorize = authorize;
            _initiatorService = initiatorService;
            _articleRepository = articleRepository;
        }

        public void Handle(ChangePrices command)
        {
            var initiator = _initiatorService.GetById(command.InitiatorId);
            var article = _articleRepository.GetById(command.ArticleId);

            _authorize.Enforce(initiator.InitiatorId, Manage.Articles(article.Owner.OwnerId));

            article.ChangePrices(initiator, command.PublicPrice, command.MemberPrice);
        }
    }
}