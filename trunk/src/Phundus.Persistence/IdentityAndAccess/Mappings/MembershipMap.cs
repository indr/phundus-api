namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using Castle.MicroKernel.Registration;
    using Core.OrganizationAndMembershipCtx.Model;
    using FluentNHibernate.Mapping;

    public class MembershipMap : ClassMap<Membership>
    {
        public MembershipMap()
        {
            Table("OrganizationMembership");

            Id(x => x.Id).GeneratedBy.Assigned();
            Version(x => x.Version);

            Map(x => x.MemberId).Column("UserId");
            Map(x => x.OrganizationId);
            References(x => x.Organization, "OrganizationId").Cascade.None();

            Map(x => x.Role);

            Map(x => x.RequestDate).ReadOnly();
            Map(x => x.IsApproved);
            Map(x => x.ApprovalDate);
            Map(x => x.IsLockedOut, "IsLocked");
            Map(x => x.LastLockoutDate);
        }
    }
}