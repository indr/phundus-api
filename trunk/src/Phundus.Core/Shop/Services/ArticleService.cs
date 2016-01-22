namespace Phundus.Shop.Services
{
    using System;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Queries;
    using Inventory.Articles.Repositories;
    using Orders.Model;
    using Phundus.Authorization;

    public interface IArticleService
    {
        Article GetById(ArticleId articleId, UserId userId);

        Article GetById(LessorId lessorId, ArticleId articleId);
    }

    public class ArticleService : IArticleService
    {
        private readonly IMemberInRole _memberInRole;
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IMemberInRole memberInRole, IArticleRepository articleRepository)
        {
            if (memberInRole == null) throw new ArgumentNullException("memberInRole");
            if (articleRepository == null) throw new ArgumentNullException("articleRepository");
            _memberInRole = memberInRole;
            _articleRepository = articleRepository;
        }

        protected ArticleService()
        {
        }

        public virtual Article GetById(ArticleId articleId, UserId userId)
        {
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (userId == null) throw new ArgumentNullException("userId");

            var article = _articleRepository.GetById(articleId.Id);
            if (article == null)
                throw new NotFoundException(String.Format("Article {0} not found.", articleId));
            return ConvertToInternal(article, userId);
        }

        private Article ConvertToInternal(Inventory.Articles.Model.Article article, UserId userId)
        {
            var price = article.PublicPrice;
            if (article.MemberPrice.HasValue && article.MemberPrice.Value > 0
                && _memberInRole.IsActiveMember(new LessorId(article.Owner.OwnerId.Id), userId))
            {
                price = article.MemberPrice.Value;
            }


            return new Article(article.Id, article.Owner, article.Name, price);
        }

        public virtual Article GetById(LessorId lessorId, ArticleId articleId)
        {
            if (lessorId == null) throw new ArgumentNullException("lessorId");
            if (articleId == null) throw new ArgumentNullException("articleId");

            var result = GetById(articleId);
            if (!Equals(result.LessorId, lessorId))
                throw new NotFoundException(String.Format("Article {0} {1} not found.", lessorId, articleId));

            return result;
        }

        public virtual Article GetById(ArticleId articleId)
        {
            if (articleId == null) throw new ArgumentNullException("articleId");

            var article = _articleRepository.GetById(articleId.Id);
            if (article == null)
                throw new NotFoundException(String.Format("Article {0} not found.", articleId));
            return new Article(article.Id, article.Owner, article.Name,
                article.PublicPrice);
        }
    }
}