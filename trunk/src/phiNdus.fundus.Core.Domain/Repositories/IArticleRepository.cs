using System.Collections.Generic;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Repositories
{
    public interface IArticleRepository : IRepository<Article>
    {
        ICollection<Article> FindAll();
        ICollection<Article> FindMany(string query);
    }
}