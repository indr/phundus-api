namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using FluentNHibernate.Mapping;
    using IdentityAccess.Queries.QueryModels;
    using NHibernate.Type;

    public class MembershipApplicationViewRowMap : ClassMap<MembershipApplicationViewRow>
    {
        public MembershipApplicationViewRowMap()
        {
            SchemaAction.Validate();

            ReadOnly();
            Table("View_IdentityAccess_Applications");

            Id(x => x.ApplicationId, "ApplicationGuid");
            Map(x => x.OrganizationId, "OrganizationGuid");
            Map(x => x.UserId, "UserGuid");
            Map(x => x.CustomMemberNumber, "CustomMemberNumber");
            Map(x => x.FirstName, "FirstName");
            Map(x => x.LastName, "LastName");
            Map(x => x.EmailAddress, "EmailAddress");
            Map(x => x.RequestedAtUtc, "RequestedAtUtc").CustomType<UtcDateTimeType>();
            Map(x => x.ApprovedAtUtc, "ApprovedAtUtc").CustomType<UtcDateTimeType>();
            Map(x => x.RejectedAtUtc, "RejectedAtUtc").CustomType<UtcDateTimeType>();
        }
    }
}