namespace Phundus.Shop.Queries.QueryModels
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Cqrs;
    using Integration.Shop;

    public class LessorQueries : NHibernateReadModelBase<LessorViewRow>, ILessorQueries
    {
        public ILessor GetByGuid(Guid lessorGuid)
        {
            var result = QueryOver().Where(p => p.LessorGuid == lessorGuid).SingleOrDefault();
            if (result == null)
                throw new NotFoundException("Lessor {0} not found.", lessorGuid);
            return result;
        }

        public IList<ILessor> Query()
        {
            return QueryOver().Where(p => p.LessorType > 0).List<ILessor>();
        }
    }
}