namespace Phundus.Core.Inventory.Articles.Repositories
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IArticleRepository : IRepository<Article>
    {
        new int Add(Article entity);
        IEnumerable<Article> ByOrganization(Guid organizationId);
        Article GetById(int articleId);
        Article GetById(Guid organizationId, int articleId);
    }
}