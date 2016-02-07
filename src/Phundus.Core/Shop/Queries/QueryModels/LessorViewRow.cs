namespace Phundus.Shop.Queries.QueryModels
{
    using System;
    using Integration.Shop;

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