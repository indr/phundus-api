namespace Phundus.Persistence.Inventory.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Inventory.Model;
    using Core.Inventory.Repositories;
    using NHibernate.Linq;

    public class ArticleRepository : RepositoryBase<Article>, IArticleRepository
    {
        private IQueryable<Article> Articles
        {
            get { return Session.Query<Article>(); }
        }

        public IEnumerable<Article> ByOrganization(int organizationId)
        {
            var query = from a in Articles where a.OrganizationId == organizationId select a;
            return query.ToFuture();
        }

        public int GetNextIdentifier()
        {
            throw new System.NotImplementedException();
        }
    }
}