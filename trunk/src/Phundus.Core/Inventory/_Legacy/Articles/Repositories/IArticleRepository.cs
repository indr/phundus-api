namespace Phundus.Core.Inventory.Articles.Repositories
{
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IArticleRepository : IRepository<Article>
    {
        new int Add(Article entity);
        IEnumerable<Article> ByOrganization(int organizationId);
        Article GetById(int articleId);
        Article GetById(int organizationId, int articleId);
    }
}