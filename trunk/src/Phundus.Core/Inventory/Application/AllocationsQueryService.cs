namespace Phundus.Core.Inventory.Application
{
    using System.Collections.Generic;
    using Common.Port.Adapter.Persistence;
    using Data;

    public interface IAllocationsQueryService
    {
        IEnumerable<AllocationData> AllAllocationsByArticleId(int articleId);
    }

    public class AllocationsQueryService : NHibernateQueryServiceBase<AllocationData>, IAllocationsQueryService
    {
        public IEnumerable<AllocationData> AllAllocationsByArticleId(int articleId)
        {
            return Query.Where(p => p.ArticleId == articleId).List();
        }
    }
}