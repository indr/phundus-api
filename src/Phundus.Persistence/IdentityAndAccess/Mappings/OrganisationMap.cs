namespace Phundus.Persistence.Mappings
{
    using Core.OrganizationAndMembershipCtx.Model;
    using FluentNHibernate.Mapping;

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