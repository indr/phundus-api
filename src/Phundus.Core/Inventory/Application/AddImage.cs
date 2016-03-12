﻿namespace Phundus.Inventory.Application
{
    using System;
    using System.IO;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using IdentityAccess.Projections;
    using Integration.IdentityAccess;
    using Model.Articles;

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
        private readonly IInitiatorService _initiatorService;
        private readonly IMemberInRole _memberInRole;

        public AddImageHandler(IMemberInRole memberInRole, IInitiatorService initiatorService,
            IArticleRepository articleRepository)
        {
            if (memberInRole == null) throw new ArgumentNullException("memberInRole");
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            if (articleRepository == null) throw new ArgumentNullException("articleRepository");
            _memberInRole = memberInRole;
            _initiatorService = initiatorService;
            _articleRepository = articleRepository;
        }

        [Transaction]
        public void Handle(AddImage command)
        {
            var initiator = _initiatorService.GetById(command.InitiatorId);
            var article = _articleRepository.GetById(command.ArticleId);

            _memberInRole.ActiveManager(article.Owner.OwnerId.Id, command.InitiatorId);

            article.AddImage(initiator, command.FileName, command.FileType, command.FileSize);
        }
    }
}