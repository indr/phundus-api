namespace Phundus.Persistence.Inventory.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Inventory.Application;
    using Core.Inventory.Domain.Model.Catalog;
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

        public Article GetById(int articleId)
        {
            var result = FindById(articleId);
            if (result == null)
                throw new ArticleNotFoundException(articleId);
            return result;
        }

        public Article GetById(int organizationId, int articleId)
        {
            var result = FindById(articleId);
            if ((result == null) || (result.OrganizationId != organizationId))
                throw new ArticleNotFoundException(articleId);
            return result;
        }

        public void Save(Article article)
        {
            Session.SaveOrUpdate(article);
        }

        public IEnumerable<Article> GetAll()
        {
            return Entities;
        }
    }
}