namespace Phundus.Inventory.Application
{
    using System;
    using System.IO;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using IdentityAccess.Projections;
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
        private readonly IMemberInRole _memberInRole;

        public AddImageHandler(IMemberInRole memberInRole, ICollaboratorService collaboratorService,
            IArticleRepository articleRepository)
        {
            _memberInRole = memberInRole;
            _collaboratorService = collaboratorService;
            _articleRepository = articleRepository;
        }

        [Transaction]
        public void Handle(AddImage command)
        {
            var initiator = _collaboratorService.Initiator(command.InitiatorId);
            var article = _articleRepository.GetById(command.ArticleId);


            _memberInRole.ActiveManager(article.Owner.OwnerId.Id, command.InitiatorId);

            // TODO: Pass manager to AddImage()
            article.AddImage(initiator, command.FileName, command.FileType, command.FileSize);
        }
    }
}