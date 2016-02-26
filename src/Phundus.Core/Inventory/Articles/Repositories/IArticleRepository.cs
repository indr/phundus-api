namespace Phundus.Inventory.Articles.Repositories
{
    using Common.Domain.Model;
    using Infrastructure;
    using Model;

    public interface IArticleRepository : IRepository<Article>
    {
        Article GetById(ArticleId articleId);
        Article GetById(ArticleShortId articleShortId);
        Article FindById(ArticleId articleId);
        Article FindById(ArticleShortId articleShortId);
    }
}