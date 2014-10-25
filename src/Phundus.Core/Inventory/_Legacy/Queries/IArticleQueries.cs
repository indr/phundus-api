namespace Phundus.Core.Inventory.Queries
{
    using System.Collections.Generic;

    public interface IArticleQueries
    {
        ArticleDto GetArticle(int id);
        IEnumerable<ArticleDto> GetArticles(int organizationId);
    }
}