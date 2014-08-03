namespace Phundus.Core.Inventory.Repositories
{
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IArticleRepository : IRepository<Article>
    {
        ICollection<Article> FindAll(int organizationId);
        ICollection<Article> FindMany(string query, int? organization, int start, int count, out int total);
    }
}