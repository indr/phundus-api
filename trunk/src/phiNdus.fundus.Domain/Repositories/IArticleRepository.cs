namespace phiNdus.fundus.Domain.Repositories
{
    using System.Collections.Generic;
    using phiNdus.fundus.Domain.Entities;
    using piNuts.phundus.Infrastructure;

    public interface IArticleRepository : IRepository<Article>
    {
        ICollection<Article> FindAll(Organization selectedOrganization);
        ICollection<Article> FindMany(string query, int? organization, int start, int count, out int total);
    }
}