namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using Core.IdentityAndAccess.Organizations.Model;
    using FluentNHibernate.Mapping;

    public class MembershipMap : ClassMap<Membership>
    {
        public MembershipMap()
        {
            Table("OrganizationMembership");

            Id(x => x.Id).GeneratedBy.Assigned();
            Version(x => x.Version);

            Map(x => x.UserId);

            References(x => x.Organization, "OrganizationId").Cascade.None();

            Map(x => x.Role).CustomType<Role>();
            Map(x => x.ApprovalDate, "ApprovalDate");
            Map(x => x.IsLocked);
        }
    }
}