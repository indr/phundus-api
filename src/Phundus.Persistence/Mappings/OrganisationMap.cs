namespace Phundus.Persistence.Mappings
{
    using Core.OrganisationCtx.DomainModel;
    using FluentNHibernate.Mapping;

    public class OrganisationMap : ClassMap<Organisation>
    {
        public OrganisationMap()
        {
            Table("Organization");
            Id(x => x.Id);
            Version(x => x.Version);
        }
    }

    public class MembershipRequestMap : ClassMap<MembershipRequest>
    {
        public MembershipRequestMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Version(x => x.Version);

            Map(x => x.OrganizationId);
            Map(x => x.MemberId);
            Map(x => x.RequestDate);
            Map(x => x.ApprovalDate);
            Map(x => x.RejectDate);
        }
    }
}