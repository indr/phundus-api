namespace Phundus.Core.Inventory.Queries
{
    using System;
    using System.Collections.Generic;

    public interface IStoreQueries
    {
        StoreDto FindByUserId(Guid userId);
    }

    public interface IArticleQueries
    {
        ArticleDto GetArticle(int id);
        IEnumerable<ArticleDto> GetArticles(Guid organizationId);
    }
}