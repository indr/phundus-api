namespace Phundus.Inventory.Application
{
    using System;
    using System.Collections.Generic;
    using Common.Querying;
    using NHibernate.Criterion;
    using Projections;

    public interface ITagsQueryService
    {
        IList<TagData> Query(string q = null);
    }

    public class TagsQueryService : QueryServiceBase<TagData>, ITagsQueryService
    {
        public IList<TagData> Query(string q = null)
        {
            var query = QueryOver();
            if (!String.IsNullOrWhiteSpace(q))
                query.WhereRestrictionOn(e => e.Name).IsInsensitiveLike(q, MatchMode.Anywhere);
            
            return query.OrderBy(x => x.Name).Asc.List();
        }
    }
}