namespace Phundus.Core.Shop.Orders.Services
{
    using Common;
    using Inventory.Articles.Repositories;
    using Model;

    public interface IArticleService
    {
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
            return new Article(article);
        }
    }
}