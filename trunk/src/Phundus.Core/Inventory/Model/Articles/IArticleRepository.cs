namespace Phundus.Inventory.Model.Articles
{
    using Common.Domain.Model;
    using Infrastructure;
    using Inventory.Articles.Model;

    public interface IArticleRepository : IRepository<Article>
    {
        Article GetById(ArticleId articleId);        
        Article FindById(ArticleId articleId);
    }
}