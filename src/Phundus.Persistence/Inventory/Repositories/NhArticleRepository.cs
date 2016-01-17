namespace Phundus.Persistence.Inventory.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using NHibernate.Linq;
    using Phundus.Inventory.Articles;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Articles.Repositories;

    public class NhArticleRepository : NhRepositoryBase<Article>, IArticleRepository
    {
        public new int Add(Article entity)
        {
            base.Add(entity);
            return entity.Id;
        }

        public IEnumerable<Article> FindByOwnerId(Guid ownerId)
        {
            return Entities.Where(p => p.Owner.OwnerId.Id == ownerId).ToFuture();
        }

        public Article GetById(int id)
        {
            var result = FindById(id);
            if (result == null)
                throw new ArticleNotFoundException(id);
            return result;
        }

        public Article GetById(Guid ownerId, int articleId)
        {
            var result = FindById(articleId);
            if ((result == null) || (result.Owner.OwnerId.Id != ownerId))
                throw new ArticleNotFoundException(articleId);
            return result;
        }

        public IEnumerable<Article> Query(InitiatorGuid currentUserId, OwnerId queryOwnerId, string query)
        {
            AssertionConcern.AssertArgumentNotNull(currentUserId, "CurrentUserId must be provided.");
            AssertionConcern.AssertArgumentNotNull(queryOwnerId, "QueryOwnerId must be provided.");
            query = query == null ? "" : query.ToLowerInvariant();

            return Entities.Where(p => p.Owner.OwnerId.Id == queryOwnerId.Id).Where(p => p.Name.ToLowerInvariant().Contains(query)).ToFuture();
        }

        public Article GetById(ArticleId articleId)
        {
            if (articleId == null) throw new ArgumentNullException("articleId");
            return GetById(articleId.Id);
        }
    }
}