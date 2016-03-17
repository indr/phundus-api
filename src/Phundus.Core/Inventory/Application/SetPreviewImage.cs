namespace Phundus.Inventory.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Articles;
    using Model.Collaborators;

    public class SetPreviewImage : ICommand
    {
        public SetPreviewImage(InitiatorId initiatorId, ArticleId articleId, string fileName)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (fileName == null) throw new ArgumentNullException("fileName");

            InitiatorId = initiatorId;
            ArticleId = articleId;
            FileName = fileName;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
        public string FileName { get; protected set; }
    }

    public class SetPreviewImageHandler : IHandleCommand<SetPreviewImage>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICollaboratorService _collaboratorService;

        public SetPreviewImageHandler(ICollaboratorService collaboratorService, IArticleRepository articleRepository)
        {
            _collaboratorService = collaboratorService;
            _articleRepository = articleRepository;
        }

        [Transaction]
        public void Handle(SetPreviewImage command)
        {
            var article = _articleRepository.GetById(command.ArticleId);
            var manager = _collaboratorService.Manager(command.InitiatorId, article.OwnerId);

            article.SetPreviewImage(manager, command.FileName);
        }
    }
}