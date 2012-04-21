using System.Collections.Generic;
using phiNdus.fundus.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.Repositories
{
    public interface IArticleRepository : IRepository<Article>
    {
        ICollection<Article> FindAll();
        ICollection<Article> FindMany(string query, int start, int count, out int total);
    }
}