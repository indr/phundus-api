namespace Phundus.Core.Shop.Services
{
    using Common;
    using Inventory.Articles.Repositories;
    using Orders.Model;

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
            return new Article(article.Id, article.OrganizationId,
                new Owner(article.Owner.OwnerId.Value, article.Owner.Name), article.Caption, article.Price);
        }
    }
}