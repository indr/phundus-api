namespace Phundus.Inventory.Application
{
    using System;
    using System.IO;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Articles;
    using Model.Collaborators;

    public class RemoveImage : ICommand
    {
        public RemoveImage(InitiatorId initiatorId, ArticleId articleId, string fileName)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (fileName == null) throw new ArgumentNullException("fileName");

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException(
                    String.Format(
                        @"The file name ""{0}"" contains invalid characters. Did you mistakenly provide path information?",
                        fileName), "fileName");

            InitiatorId = initiatorId;
            ArticleId = articleId;
            FileName = fileName;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
        public string FileName { get; protected set; }
    }

    public class RemoveImageHandler : IHandleCommand<RemoveImage>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICollaboratorService _collaboratorService;

        public RemoveImageHandler(ICollaboratorService collaboratorService,
            IArticleRepository articleRepository)
        {
            _collaboratorService = collaboratorService;
            _articleRepository = articleRepository;
        }

        [Transaction]
        public void Handle(RemoveImage command)
        {
            var article = _articleRepository.GetById(command.ArticleId);
            var initiator = _collaboratorService.Manager(command.InitiatorId, article.OwnerId);

            article.RemoveImage(initiator, command.FileName);
        }
    }
}