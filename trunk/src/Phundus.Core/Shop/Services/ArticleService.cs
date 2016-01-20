namespace Phundus.Shop.Services
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Inventory.Articles.Repositories;
    using Orders.Model;

    public interface IArticleService
    {
        Article GetById(ArticleId articleId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="articleId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Article GetById(OwnerId ownerId, ArticleId articleId);

        Article GetById(LessorId lessorId, ArticleId articleId);
    }

    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            AssertionConcern.AssertArgumentNotNull(articleRepository, "ArticleRepository must be provided.");

            _articleRepository = articleRepository;
        }

        public Article GetById(ArticleId articleId)
        {
            if (articleId == null) throw new ArgumentNullException("articleId");
            
            var article = _articleRepository.GetById(articleId.Id);
            if (article == null)
                throw new NotFoundException(String.Format("Article {0} not found.", articleId));
            return ToArticleValueObject(article);
        }

        public Article GetById(OwnerId ownerId, ArticleId articleId)
        {
            AssertionConcern.AssertArgumentNotNull(ownerId, "OwnerId must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "ArticleId must be provided.");
            
            var result = GetById(articleId);
            if (!Equals(result.Owner.OwnerId, ownerId))
                throw new NotFoundException(String.Format("Article {0} {1} not found.", ownerId, articleId));

            return result;
        }

        public Article GetById(LessorId lessorId, ArticleId articleId)
        {
            return GetById(new OwnerId(lessorId.Id), articleId);
        }

        private static Article ToArticleValueObject(Inventory.Articles.Model.Article article)
        {
            return new Article(article.Id, new Owner(article.Owner.OwnerId, article.Owner.Name), article.Name, article.Price);
        }
    }
}