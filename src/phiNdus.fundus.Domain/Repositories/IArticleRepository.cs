using System.Collections.Generic;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Domain.Repositories
{
    using piNuts.phundus.Infrastructure.Obsolete;

    public interface IArticleRepository : IRepository<Article>
    {
        ICollection<Article> FindAll(Organization selectedOrganization);
        ICollection<Article> FindMany(string query, int start, int count, out int total);
    }
}