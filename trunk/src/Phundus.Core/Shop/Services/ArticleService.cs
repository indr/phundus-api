namespace Phundus.Core.Shop.Services
{
    using System;
    using Common;
    using Inventory.Articles.Repositories;
    using Orders.Model;

    public interface IArticleService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Article GetById(int articleId);
    }

    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            AssertionConcern.AssertArgumentNotNull(articleRepository, "ArticleRepository must be provided.");

            _articleRepository = articleRepository;
        }

        public Article GetById(int articleId)
        {
            var article = _articleRepository.GetById(articleId);
            if (article == null)
                throw new NotFoundException(String.Format("Article with id {0} not found.", articleId));
            return ToArticleValueObject(article);
        }

        private static Article ToArticleValueObject(Inventory.Articles.Model.Article article)
        {
            return new Article(article.Id, new Owner(article.Owner.OwnerId.Id, article.Owner.Name), article.Name, article.Price);
        }
    }
}