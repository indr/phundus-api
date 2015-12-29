namespace Phundus.Core.Inventory.Queries
{
    using System;
    using System.Collections.Generic;

    public interface IArticleQueries
    {
        ArticleDto GetById(int id);
        IEnumerable<ArticleDto> FindByOwnerId(Guid ownerId);
    }
}