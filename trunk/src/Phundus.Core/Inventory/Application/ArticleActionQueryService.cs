namespace Phundus.Inventory.Application
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Querying;
    using Projections;

    public interface IArticleActionQueryService
    {
        IEnumerable<ArticleActionData> GetActions(ArticleId articleId);
    }

    public class ArticleActionQueryService : QueryServiceBase<ArticleActionData>, IArticleActionQueryService
    {
        public IEnumerable<ArticleActionData> GetActions(ArticleId articleId)
        {
            if (articleId == null) throw new ArgumentNullException("articleId");

            return QueryOver().Where(p => p.ArticleId == articleId.Id).OrderBy(p => p.OccuredOnUtc).Desc.List();
        }
    }
}