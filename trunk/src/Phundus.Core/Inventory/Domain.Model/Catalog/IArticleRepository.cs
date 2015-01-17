namespace Phundus.Core.Inventory.Domain.Model.Catalog
{
    using System.Collections.Generic;
    using Infrastructure;

    public interface IArticleRepository : IRepository<Article>
    {
        new int Add(Article entity);
        IEnumerable<Article> ByOrganization(int organizationId);
        Article GetById(int articleId);
        Article GetById(int organizationId, int articleId);
        void Save(Article article);
        IEnumerable<Article> GetAll();
    }
}