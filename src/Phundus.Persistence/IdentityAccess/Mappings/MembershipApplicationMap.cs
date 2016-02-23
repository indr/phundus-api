namespace Phundus.Persistence.IdentityAccess.Mappings
{
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Phundus.IdentityAccess.Organizations.Model;

    public class MembershipApplicationMap : ClassMap<MembershipApplication>
    {
        public MembershipApplicationMap()
        {
            SchemaAction.Validate();

            Table("Dm_IdentityAccess_Application");

            CompositeId(x => x.MembershipApplicationId)
                .KeyProperty(x => x.Id, "Id");
            Version(x => x.Version);

            Component(x => x.OrganizationId, a =>
                a.Map(x => x.Id, "OrganizationGuid"));
            
            Component(x => x.UserId, a =>
                a.Map(x => x.Id, "UserGuid"));

            Map(x => x.RequestedAtUtc, "RequestDate").CustomType<UtcDateTimeType>();
            Map(x => x.ApprovedAtUtc, "ApprovalDate").CustomType<UtcDateTimeType>();
            Map(x => x.RejectedAtUtc, "RejectDate").CustomType<UtcDateTimeType>();
        }
    }
}