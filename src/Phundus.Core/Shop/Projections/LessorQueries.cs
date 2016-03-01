namespace Phundus.Shop.Projections
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Querying;
    using Cqrs;

    public interface ILessorQueries
    {
        LessorData GetByGuid(Guid lessorId);
        IList<LessorData> Query();
    }

    public class LessorQueries : QueryBase<LessorData>, ILessorQueries
    {
        public LessorData GetByGuid(Guid lessorId)
        {
            var result = QueryOver().Where(p => p.LessorId == lessorId).SingleOrDefault();
            if (result == null)
                throw new NotFoundException("Lessor {0} not found.", lessorId);
            return result;
        }

        public IList<LessorData> Query()
        {
            return QueryOver().Where(p => p.Type == LessorType.Organization).OrderBy(p => p.Name).Asc.List();
        }
    }
}