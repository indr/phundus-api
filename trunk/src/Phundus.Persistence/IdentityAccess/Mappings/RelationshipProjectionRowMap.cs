namespace Phundus.Persistence.IdentityAccess.Mappings
{
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Phundus.IdentityAccess.Application;

    public class RelationshipProjectionRowMap : ClassMap<RelationshipData>
    {
        public RelationshipProjectionRowMap()
        {
            SchemaAction.All();

            Table("Es_IdentityAccess_Relationships");

            Id(x => x.RowGuid, "RowGuid").GeneratedBy.GuidComb();
            Map(x => x.OrganizationGuid, "OrganizationGuid").Not.Nullable().UniqueKey("OrganizationGuid_UserGuid");
            Map(x => x.UserGuid, "UserGuid").Not.Nullable().UniqueKey("OrganizationGuid_UserGuid");
            Map(x => x.Timestamp, "Timestamp").Not.Nullable().CustomType<UtcDateTimeType>();
            Map(x => x.Status, "Status").Not.Nullable();
        }
    }
}