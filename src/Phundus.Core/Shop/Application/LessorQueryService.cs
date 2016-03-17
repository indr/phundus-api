namespace Phundus.Shop.Application
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Querying;

    public interface ILessorQueryService
    {
        LessorData GetById(Guid lessorId);
        IList<LessorData> Query();
    }

    public class LessorQueryService : QueryServiceBase<LessorData>, ILessorQueryService
    {
        public LessorData GetById(Guid lessorId)
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