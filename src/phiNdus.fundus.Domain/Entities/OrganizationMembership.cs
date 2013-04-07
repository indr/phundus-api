namespace phiNdus.fundus.Domain.Entities
{
    public class OrganizationMembership : EntityBase
    {
        public virtual User User { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual int Role { get; set; }
    }
}