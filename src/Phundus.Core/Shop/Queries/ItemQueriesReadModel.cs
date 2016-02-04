namespace Phundus.Shop.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Cqrs;
    using NHibernate.SqlCommand;
    using Projections;

    public interface IItemQueries
    {
        IList<ShopArticleSearchResultDto> Query(string globalSearch);
        ShopItemProjectionRow Get(Guid itemGuid);
    }

    public class ItemQueriesReadModel : ReadModelBase, IItemQueries
    {
        public IList<ShopArticleSearchResultDto> Query(string globalSearch)
        {
            return new List<ShopArticleSearchResultDto>();
        }

        public ShopItemProjectionRow Get(Guid itemGuid)
        {
            ShopItemFilesProjectionRow file = null;
            ShopItemImagesProjectionRow image = null;
            var result = Session.QueryOver<ShopItemProjectionRow>()
                .Where(p => p.ArticleGuid == itemGuid)
                .JoinAlias(x => x.Documents, () => file, JoinType.LeftOuterJoin)
                .JoinAlias(x => x.Images, () => image, JoinType.LeftOuterJoin)
                .SingleOrDefault();
            if (result == null)
                throw new NotFoundException("Shop item {0} not found.", itemGuid);
            return result;
        }
    }
}