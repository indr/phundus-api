namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using Castle.MicroKernel.Registration;
    using Core.IdentityAndAccess.Organizations.Model;
    using FluentNHibernate.Mapping;

    public class MembershipMap : ClassMap<Membership>
    {
        public MembershipMap()
        {
            Table("OrganizationMembership");

            Id(x => x.Id).GeneratedBy.Assigned();
            Version(x => x.Version);

            Map(x => x.MemberId).Column("UserId");
            
            References(x => x.Organization, "OrganizationId").Cascade.None();

            Map(x => x.Role);
            Map(x => x.ApprovalDate, "ApprovalDate");
        }
    }
}