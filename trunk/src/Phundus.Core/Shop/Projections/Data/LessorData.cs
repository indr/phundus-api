namespace Phundus.Shop.Projections
{
    using System;

    public class LessorData
    {
        public virtual Guid LessorId { get; set; }
        public virtual LessorType Type { get; set; }
        public virtual string Name { get; set; }
        public virtual string PostalAddress { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual bool PublicRental { get; set; }

        public enum LessorType
        {
            Organization,
            User
        }
    }
}