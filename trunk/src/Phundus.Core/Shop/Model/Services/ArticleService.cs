namespace Phundus.Shop.Services
{
    using System;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Queries;
    using Inventory.Articles.Repositories;
    using Model;
    using Orders.Model;

    public interface IArticleService
    {
        Article GetById(ArticleId articleId, UserId userId);
        Article GetById(LessorId lessorId, ArticleId articleId, LesseeId lesseeId);
    }

    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ILessorService _lessorService;
        private readonly IMemberInRole _memberInRole;

        public ArticleService(IMemberInRole memberInRole, IArticleRepository articleRepository, ILessorService lessorService)
        {
            if (memberInRole == null) throw new ArgumentNullException("memberInRole");
            if (articleRepository == null) throw new ArgumentNullException("articleRepository");
            if (lessorService == null) throw new ArgumentNullException("lessorService");

            _memberInRole = memberInRole;
            _articleRepository = articleRepository;
            _lessorService = lessorService;
        }

        protected ArticleService()
        {
        }

        public virtual Article GetById(ArticleId articleId, UserId userId)
        {
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (userId == null) throw new ArgumentNullException("userId");

            var article = _articleRepository.GetById(articleId);
            return ConvertToInternal(article, userId);
        }

        public virtual Article GetById(LessorId lessorId, ArticleId articleId, LesseeId lesseeId)
        {
            if (lessorId == null) throw new ArgumentNullException("lessorId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (lesseeId == null) throw new ArgumentNullException("lesseeId");

            var result = GetById(articleId, new UserId(lesseeId.Id));
            if (!Equals(result.LessorId, lessorId))
                throw new NotFoundException(String.Format("Article {0} {1} not found.", lessorId, articleId));

            return result;
        }

        private Article ConvertToInternal(Inventory.Articles.Model.Article article, UserId userId)
        {
            var lessor = _lessorService.GetById(new LessorId(article.Owner.OwnerId.Id));
            var price = article.PublicPrice;
            if (article.MemberPrice.HasValue && article.MemberPrice.Value > 0
                && _memberInRole.IsActiveMember(lessor.LessorId, userId))
            {
                price = article.MemberPrice.Value;
            }

            return new Article(article.ArticleShortId, article.ArticleId, lessor, article.Name, price);
        }
    }
}