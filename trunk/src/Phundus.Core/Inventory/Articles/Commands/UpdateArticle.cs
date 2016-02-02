namespace Phundus.Inventory.Articles.Commands
{
    using System;
    using Authorization;
    using Authorize;
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;
    using Integration.IdentityAccess;
    using Model;
    using Repositories;

    public class UpdateArticle
    {
        public UpdateArticle(InitiatorId initiatorId, int articleId, string name, string brand, string color,
            int grossStock)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (name == null) throw new ArgumentNullException("name");
            InitiatorId = initiatorId;
            ArticleId = articleId;
            Name = name;
            Brand = brand;
            Color = color;
            GrossStock = grossStock;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public int ArticleId { get; protected set; }

        public string Name { get; protected set; }
        public string Brand { get; protected set; }
        public string Color { get; protected set; }
        public int GrossStock { get; protected set; }
    }

    public class UpdateArticleHandler : IHandleCommand<UpdateArticle>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IAuthorize _authorize;
        private readonly IInitiatorService _initiatorService;

        public UpdateArticleHandler(IAuthorize authorize, IInitiatorService initiatorService,
            IArticleRepository articleRepository)
        {
            if (authorize == null) throw new ArgumentNullException("authorize");
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            if (articleRepository == null) throw new ArgumentNullException("articleRepository");
            _authorize = authorize;
            _initiatorService = initiatorService;
            _articleRepository = articleRepository;
        }

        public void Handle(UpdateArticle command)
        {
            var initiator = _initiatorService.GetActiveById(command.InitiatorId);
            var article = _articleRepository.GetById(command.ArticleId);

            _authorize.Enforce(initiator.InitiatorId, Manage.Articles(article.Owner.OwnerId));

            article.ChangeDetails(initiator, command.Name, command.Brand, command.Color);
            article.ChangeGrossStock(initiator, command.GrossStock);
        }
    }
}