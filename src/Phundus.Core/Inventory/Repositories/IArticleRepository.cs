namespace Phundus.Core.Inventory.Repositories
{
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IArticleRepository : IRepository<Article>
    {
        IEnumerable<Article> ByOrganization(int organizationId);
        int GetNextIdentifier();
    }
}