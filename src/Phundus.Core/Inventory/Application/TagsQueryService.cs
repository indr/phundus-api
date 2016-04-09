namespace Phundus.Inventory.Application
{
    using System.Collections.Generic;
    using Common.Querying;
    using Projections;

    public interface ITagsQueryService
    {
        IList<TagData> Query();
    }

    public class TagsQueryService : QueryServiceBase<TagData>, ITagsQueryService
    {
        public IList<TagData> Query()
        {
            return QueryOver().OrderBy(x => x.Name).Asc.List();
        }
    }
}