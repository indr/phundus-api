namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using Core.IdentityAndAccess.Organizations.Model;
    using FluentNHibernate.Mapping;

    public class MembershipRequestMap : ClassMap<MembershipRequest>
    {
        public MembershipRequestMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Version(x => x.Version);

            Map(x => x.OrganizationId);
            Map(x => x.UserId);
            Map(x => x.RequestDate);
            Map(x => x.ApprovalDate);
            Map(x => x.RejectDate);
        }
    }
}