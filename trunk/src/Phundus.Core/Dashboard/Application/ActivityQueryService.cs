namespace Phundus.Core.Dashboard.Application
{
    using System.Collections.Generic;
    using Common.Port.Adapter.Persistence;
    using Data;
    
    public interface IActivityQueryService
    {
        IEnumerable<ActivityData> FindMostRecent20();
    }

    public class ActivityQueryService : NHibernateQueryServiceBase<ActivityData>, IActivityQueryService
    {
        public IEnumerable<ActivityData> FindMostRecent20()
        {
            return Query.OrderBy(p => p.OccuredOnUtc).Desc.Take(20).List();
        }
    }
}