using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Repositories
{
    public class ArticleRepository : NHRepository<Article>, IArticleRepository
    {
        private IQueryable<Article> Articles
        {
            get { return Session.Query<Article>(); }
        }

        #region IArticleRepository Members

        public ICollection<Article> FindAll()
        {
            var query = from a in Articles select a;
            return query.ToList();
        }

        #endregion
    }
}