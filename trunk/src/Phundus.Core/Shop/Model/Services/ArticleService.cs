namespace Phundus.Shop.Services
{
    using System;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Queries;
    using Inventory.Articles.Repositories;
    using Orders.Model;

    public interface IArticleService
    {
        Article GetById(ArticleShortId articleShortId, UserId userId);

        Article GetById(LessorId lessorId, ArticleShortId articleShortId, LesseeId lesseeId);
    }

    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMemberInRole _memberInRole;

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

        public virtual Article GetById(ArticleShortId articleShortId, UserId userId)
        {
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (userId == null) throw new ArgumentNullException("userId");

            var article = _articleRepository.GetById(articleShortId.Id);
            if (article == null)
                throw new NotFoundException(String.Format("Article {0} not found.", articleShortId));
            return ConvertToInternal(article, userId);
        }

        public virtual Article GetById(LessorId lessorId, ArticleShortId articleShortId, LesseeId lesseeId)
        {
            if (lessorId == null) throw new ArgumentNullException("lessorId");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (lesseeId == null) throw new ArgumentNullException("lesseeId");

            var result = GetById(articleShortId, new UserId(lesseeId.Id));
            if (!Equals(result.LessorId, lessorId))
                throw new NotFoundException(String.Format("Article {0} {1} not found.", lessorId, articleShortId));

            return result;
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
    }
}