namespace Phundus.Inventory.Articles.Commands
{
    using System;
    using System.IO;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Queries;
    using Repositories;

    public class RemoveImage
    {
        public RemoveImage(InitiatorId initiatorId, ArticleId articleId, string fileName)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (fileName == null) throw new ArgumentNullException("fileName");

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException(String.Format(@"The file name ""{0}"" contains invalid characters. Did you mistakenly provide path information?", fileName), "fileName");

            InitiatorId = initiatorId;
            ArticleId = articleId;
            FileName = fileName;
        }

        public InitiatorId InitiatorId { get; set; }
        public ArticleId ArticleId { get; set; }
        public string FileName { get; set; }
    }

    public class RemoveImageHandler : IHandleCommand<RemoveImage>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMemberInRole _memberInRole;

        public RemoveImageHandler(IMemberInRole memberInRole, IArticleRepository articleRepository)
        {
            if (memberInRole == null) throw new ArgumentNullException("memberInRole");
            if (articleRepository == null) throw new ArgumentNullException("articleRepository");
            _memberInRole = memberInRole;
            _articleRepository = articleRepository;
        }

        public void Handle(RemoveImage command)
        {
            var article = _articleRepository.GetById(command.ArticleId.Id);

            _memberInRole.ActiveManager(article.Owner.OwnerId.Id, command.InitiatorId);

            article.RemoveImage(null, command.FileName);
        }
    }
}