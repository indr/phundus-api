namespace Phundus.Shop.Projections
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Cqrs;

    public interface ILessorQueries
    {
        LessorViewRow GetByGuid(Guid lessorId);
        IList<LessorViewRow> Query();
    }

    public class LessorsProjection : ProjectionBase<LessorViewRow>, ILessorQueries
    {
        public LessorViewRow GetByGuid(Guid lessorId)
        {
            var result = SingleOrDefault(p => p.LessorGuid == lessorId);
            if (result == null)
                throw new NotFoundException("Lessor {0} not found.", lessorId);
            return result;
        }

        public IList<LessorViewRow> Query()
        {
            return QueryOver().Where(p => p.LessorType >= 0).OrderBy(p => p.Name).Asc.List();
        }
    }

    public class LessorViewRow
    {
        public virtual Guid LessorGuid { get; protected set; }
        public virtual int LessorType { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Address { get; protected set; }
        public virtual string PhoneNumber { get; protected set; }
        public virtual string EmailAddress { get; protected set; }
        public virtual bool PublicRental { get; protected set; }
    }
}