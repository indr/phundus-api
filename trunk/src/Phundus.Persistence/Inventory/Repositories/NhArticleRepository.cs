namespace Phundus.Persistence.Inventory.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Inventory.Model;
    using Core.Inventory.Repositories;
    using NHibernate.Linq;

    public class NhArticleRepository : NhRepositoryBase<Article>, IArticleRepository
    {
        public new int Add(Article entity)
        {
            base.Add(entity);
            return entity.Id;
        }

        public IEnumerable<Article> ByOrganization(int organizationId)
        {
            return Entities.Where(p => p.OrganizationId == organizationId).ToFuture();
        }
    }
}