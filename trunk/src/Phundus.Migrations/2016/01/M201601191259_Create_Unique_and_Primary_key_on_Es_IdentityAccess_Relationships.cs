namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;
    using FluentMigrator.Runner.Extensions;

    [Migration(201601191259)]
    public class M201601191259_Create_Unique_and_Primary_key_on_Es_IdentityAccess_Relationships : MigrationBase
    {
        public override void Up()
        {
            EmptyTableAndResetTracker("Es_IdentityAccess_Relationships", "Phundus.IdentityAccess.Queries.EventSourcedViewsUpdater.EsRelationshipsUpdater");
            Rename.Column("RelationshipGuid").OnTable("Es_IdentityAccess_Relationships").To("RowGuid");
            Create.PrimaryKey().OnTable("Es_IdentityAccess_Relationships").Column("RowGuid").NonClustered();
            Create.UniqueConstraint().OnTable("Es_IdentityAccess_Relationships").Columns(new[] { "OrganizationGuid", "UserGuid" }).NonClustered();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}