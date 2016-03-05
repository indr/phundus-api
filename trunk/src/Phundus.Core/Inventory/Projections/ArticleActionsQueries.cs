namespace Phundus.Inventory.Projections
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Querying;

    public interface IArticleActionsQueries
    {
        IEnumerable<ArticleActionData> GetActions(ArticleId articleId);
    }

    public class ArticleActionsQueries : QueryBase<ArticleActionData>, IArticleActionsQueries
    {
        public IEnumerable<ArticleActionData> GetActions(ArticleId articleId)
        {
            if (articleId == null) throw new ArgumentNullException("articleId");

            return QueryOver().Where(p => p.ArticleId == articleId.Id).OrderBy(p => p.OccuredOnUtc).Desc.List();
        }
    }
}