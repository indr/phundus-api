namespace Phundus.Inventory.Articles.Commands
{
    using System;
    using System.IO;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Queries;
    using Integration.IdentityAccess;
    using Repositories;

    public class AddImage : ICommand
    {
        public AddImage(InitiatorId initiatorId, ArticleShortId articleShortId, string fileName, string fileType, long length)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (fileType == null) throw new ArgumentNullException("fileType");

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException(
                    String.Format(
                        @"The file name ""{0}"" contains invalid characters. Did you mistakenly provide path information?",
                        fileName), "fileName");

            InitiatorId = initiatorId;
            ArticleShortId = articleShortId;
            FileName = fileName;
            FileType = fileType;
            Length = length;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public ArticleShortId ArticleShortId { get; protected set; }
        public long Length { get; protected set; }
        public string FileName { get; protected set; }
        public string FileType { get; protected set; }

        public int ResultingImageId { get; set; }
    }

    public class AddImageHandler : IHandleCommand<AddImage>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMemberInRole _memberInRole;
        private readonly IInitiatorService _initiatorService;

        public AddImageHandler(IMemberInRole memberInRole, IInitiatorService initiatorService, IArticleRepository articleRepository)
        {
            if (memberInRole == null) throw new ArgumentNullException("memberInRole");
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            if (articleRepository == null) throw new ArgumentNullException("articleRepository");
            _memberInRole = memberInRole;
            _initiatorService = initiatorService;
            _articleRepository = articleRepository;
        }

        public void Handle(AddImage command)
        {
            var initiator = _initiatorService.GetActiveById(command.InitiatorId);
            var article = _articleRepository.GetById(command.ArticleShortId);

            _memberInRole.ActiveManager(article.Owner.OwnerId.Id, command.InitiatorId);

            var image = article.AddImage(initiator, command.FileName, command.FileType, command.Length);
            command.ResultingImageId = image.Id;
        }
    }
}