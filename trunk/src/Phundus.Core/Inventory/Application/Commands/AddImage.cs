namespace Phundus.Inventory.Application
{
    using System;
    using System.IO;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Articles;
    using Model.Collaborators;

    public class AddImage : ICommand
    {
        public AddImage(InitiatorId initiatorId, ArticleId articleId, string fileName, string fileType, long fileSize)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (fileType == null) throw new ArgumentNullException("fileType");
            if (fileSize <= 0) throw new ArgumentOutOfRangeException("fileSize");

            AssertNoInvalidFileNameChars(fileName);

            InitiatorId = initiatorId;
            ArticleId = articleId;
            FileName = fileName;
            FileType = fileType;
            FileSize = fileSize;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
        public long FileSize { get; protected set; }
        public string FileName { get; protected set; }
        public string FileType { get; protected set; }

        private void AssertNoInvalidFileNameChars(string fileName)
        {
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException(String.Format(
                    @"The file name ""{0}"" contains invalid characters. Did you mistakenly provide path information?",
                    fileName), "fileName");
        }
    }

    public class AddImageHandler : IHandleCommand<AddImage>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICollaboratorService _collaboratorService;

        public AddImageHandler(ICollaboratorService collaboratorService, IArticleRepository articleRepository)
        {
            _collaboratorService = collaboratorService;
            _articleRepository = articleRepository;
        }

        [Transaction]
        public void Handle(AddImage command)
        {
            var article = _articleRepository.GetById(command.ArticleId);
            var manager = _collaboratorService.Manager(command.InitiatorId, article.Owner.OwnerId);
            
            article.AddImage(manager, command.FileName, command.FileType, command.FileSize);
        }
    }
}