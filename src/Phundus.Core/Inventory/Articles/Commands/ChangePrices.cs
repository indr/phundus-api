namespace Phundus.Inventory.Articles.Commands
{
    using System;
    using Authorization;
    using Common.Domain.Model;
    using Cqrs;
    using Integration.IdentityAccess;
    using Phundus.Authorization;
    using Repositories;

    public class ChangePrices
    {
        public InitiatorId InitiatorId { get; set; }
        public int ArticleId { get; set; }
        public decimal PublicPrice { get; set; }
        public decimal? MemberPrice { get; set; }

        public ChangePrices(InitiatorId initiatorId, int articleId, decimal publicPrice, decimal? memberPrice)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            InitiatorId = initiatorId;
            ArticleId = articleId;
            PublicPrice = publicPrice;
            MemberPrice = memberPrice;
        }
    }

    public class ChangePricesHandler : IHandleCommand<ChangePrices>
    {
        private readonly IAuthorize _authorize;
        private readonly IInitiatorService _initiatorService;
        private readonly IArticleRepository _articleRepository;

        public ChangePricesHandler(IAuthorize authorize, IInitiatorService initiatorService, IArticleRepository articleRepository)
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
            var initiator = _initiatorService.GetActiveById(command.InitiatorId);
            var article = _articleRepository.GetById(command.ArticleId);

            _authorize.Enforce(initiator.InitiatorId, Manage.Articles(article.Owner.OwnerId));

            article.ChangePrices(initiator, command.PublicPrice, command.MemberPrice);
        }
    }
}