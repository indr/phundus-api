namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using Core.IdentityAndAccess.Organizations.Model;
    using FluentNHibernate.Mapping;

    public class MembershipApplicationMap : ClassMap<MembershipApplication>
    {
        public MembershipApplicationMap()
        {
            SchemaAction.Validate();

            Table("Dm_IdentityAccess_Application");
            Id(x => x.Id).GeneratedBy.Assigned();
            Version(x => x.Version);

            Map(x => x.OrganizationId, "OrganizationGuid");
            Map(x => x.UserId, "MemberId").ReadOnly();
            References(x => x.User, "MemberId").Cascade.None();

            Map(x => x.RequestDate);
            Map(x => x.ApprovalDate);
            Map(x => x.RejectDate);
        }
    }
}