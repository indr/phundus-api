namespace Phundus.Persistence.Inventory.Repositories
{
    using System;
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

        public IEnumerable<Article> ByOrganization(Guid organizationId)
        {
            return Entities.Where(p => p.Owner.OwnerId.Value == organizationId).ToFuture();
        }

        public Article GetById(int id)
        {
            var result = FindById(id);
            if (result == null)
                throw new ArticleNotFoundException(id);
            return result;
        }

        public Article GetById(Guid organizationId, int articleId)
        {
            var result = FindById(articleId);
            if ((result == null) || (result.Owner.OwnerId.Value != organizationId))
                throw new ArticleNotFoundException(articleId);
            return result;
        }
    }
}