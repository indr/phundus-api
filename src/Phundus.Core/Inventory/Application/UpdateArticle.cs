namespace Phundus.Inventory.Application
{
    using System;
    using Authorization;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Articles;
    using Model.Collaborators;

    public class UpdateArticle : ICommand
    {
        public UpdateArticle(InitiatorId initiatorId, ArticleId articleId, string name, string brand, string color,
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
        public ArticleId ArticleId { get; protected set; }
        public string Name { get; protected set; }
        public string Brand { get; protected set; }
        public string Color { get; protected set; }
        public int GrossStock { get; protected set; }
    }

    public class UpdateArticleHandler : IHandleCommand<UpdateArticle>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IAuthorize _authorize;
        private readonly ICollaboratorService _collaboratorService;

        public UpdateArticleHandler(IAuthorize authorize, ICollaboratorService collaboratorService,
            IArticleRepository articleRepository)
        {
            _authorize = authorize;
            _collaboratorService = collaboratorService;
            _articleRepository = articleRepository;
        }

        [Transaction]
        public void Handle(UpdateArticle command)
        {
            var article = _articleRepository.GetById(command.ArticleId);
            var manager = _collaboratorService.Manager(command.InitiatorId, article.OwnerId);

            article.ChangeDetails(manager, command.Name, command.Brand, command.Color);
            article.ChangeGrossStock(manager, command.GrossStock);
        }
    }
}