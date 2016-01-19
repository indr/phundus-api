namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using System;
    using FluentNHibernate.Mapping;
    using IdentityAccess.Queries.EventSourcedViewsUpdater;
    using NHibernate.Type;

    public class RelationshipViewRowMap : ClassMap<RelationshipViewRow>
    {
        public RelationshipViewRowMap()
        {
            SchemaAction.Validate();

            Table("Es_IdentityAccess_Relationships");

            Id(x => x.RelationshipGuid, "RelationshipGuid").GeneratedBy.GuidComb();
            Map(x => x.OrganizationGuid, "OrganizationGuid").Not.Nullable();
            Map(x => x.UserGuid, "UserGuid").Not.Nullable();
            Map(x => x.Timestamp, "Timestamp").Not.Nullable().CustomType<UtcDateTimeType>();
            Map(x => x.Status, "Status").Not.Nullable();
        }
    }
}