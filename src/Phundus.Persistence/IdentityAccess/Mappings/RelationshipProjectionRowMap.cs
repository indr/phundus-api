namespace Phundus.Persistence.IdentityAccess.Mappings
{
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Phundus.IdentityAccess.Projections;

    public class RelationshipProjectionRowMap : ClassMap<RelationshipProjectionRow>
    {
        public RelationshipProjectionRowMap()
        {
            SchemaAction.Validate();

            Table("Es_IdentityAccess_Relationships");

            Id(x => x.RowGuid, "RowGuid").GeneratedBy.GuidComb();
            Map(x => x.OrganizationGuid, "OrganizationGuid").Not.Nullable();
            Map(x => x.UserGuid, "UserGuid").Not.Nullable();
            Map(x => x.Timestamp, "Timestamp").Not.Nullable().CustomType<UtcDateTimeType>();
            Map(x => x.Status, "Status").Not.Nullable();
        }
    }
}