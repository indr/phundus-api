namespace Phundus.Shop.Queries.QueryModels
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Cqrs;
    using Integration.Shop;

    public interface ILessorQueries
    {
        ILessor GetByGuid(Guid lessorId);
        IList<ILessor> Query();
    }

    public class LessorQueries : ProjectionBase<LessorViewRow>, ILessorQueries
    {
        public ILessor GetByGuid(Guid lessorId)
        {
            var result = Single(p => p.LessorGuid == lessorId);
            if (result == null)
                throw new NotFoundException("Lessor {0} not found.", lessorId);
            return result;
        }

        public new IList<ILessor> Query()
        {
            return base.Query().Where(p => p.LessorType >= 0).OrderBy(p => p.Name).Asc.List<ILessor>();
        }
    }

    public class LessorViewRow : ILessor
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