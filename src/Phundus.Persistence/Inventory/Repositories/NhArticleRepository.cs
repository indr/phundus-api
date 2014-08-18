namespace Phundus.Persistence.Inventory.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Inventory.Articles;
    using Core.Inventory.Articles.Model;
    using Core.Inventory.Articles.Repositories;
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

        public Article GetById(int id)
        {
            var result = FindById(id);
            if (result == null)
                throw new ArticleNotFoundException(id);
            return result;
        }
    }
}