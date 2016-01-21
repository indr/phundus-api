namespace Phundus.Inventory.Articles.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Queries;
    using IdentityAccess.Queries.ReadModels;
    using Repositories;

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
        private readonly IMemberInRole _memberInRole;
        private readonly IArticleRepository _articleRepository;

        public SetPreviewImageHandler(IMemberInRole memberInRole, IArticleRepository articleRepository)
        {
            if (memberInRole == null) throw new ArgumentNullException("memberInRole");
            if (articleRepository == null) throw new ArgumentNullException("articleRepository");
            _memberInRole = memberInRole;
            _articleRepository = articleRepository;
        }

        public void Handle(SetPreviewImage command)
        {
            var article = _articleRepository.GetById(command.ArticleId);

            _memberInRole.ActiveManager(article.Owner.OwnerId, command.InitiatorId);

            article.SetPreviewImage(command.FileName);
        }
    }
}