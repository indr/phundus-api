﻿namespace Phundus.Inventory.Articles.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Queries;
    using Repositories;

    public class AddImage : ICommand
    {
        public AddImage(InitiatorGuid initiatorId, ArticleId articleId, string fileName, string fileType, long length)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (fileType == null) throw new ArgumentNullException("fileType");
            InitiatorId = initiatorId;
            ArticleId = articleId;
            FileName = fileName;
            FileType = fileType;
            Length = length;
        }

        public InitiatorGuid InitiatorId { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
        public long Length { get; protected set; }
        public string FileName { get; protected set; }
        public string FileType { get; protected set; }

        public int ResultingImageId { get; set; }
    }

    public class AddImageHandler : IHandleCommand<AddImage>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMemberInRole _memberInRole;

        public AddImageHandler(IMemberInRole memberInRole, IArticleRepository articleRepository)
        {
            if (memberInRole == null) throw new ArgumentNullException("memberInRole");
            if (articleRepository == null) throw new ArgumentNullException("articleRepository");
            _memberInRole = memberInRole;
            _articleRepository = articleRepository;
        }

        public void Handle(AddImage command)
        {
            var article = _articleRepository.GetById(command.ArticleId.Id);

            _memberInRole.ActiveChief(article.Owner.OwnerId.Id, command.InitiatorId);

            var image = article.AddImage(command.FileName, command.FileType, command.Length);
            command.ResultingImageId = image.Id;
        }
    }
}