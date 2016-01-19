namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201601190729)]
    // ReSharper disable once InconsistentNaming
    public class M201601190729_Create_Table_Es_IdentityAccess_Relationships : MigrationBase
    {
        public override void Up()
        {
            Create.Table("Es_IdentityAccess_Relationships").InSchema(SchemaName)
                .WithColumn("RelationshipGuid").AsGuid().NotNullable()
                .WithColumn("OrganizationGuid").AsGuid().NotNullable()
                .WithColumn("UserGuid").AsGuid().NotNullable()
                .WithColumn("Timestamp").AsDateTime().NotNullable()
                .WithColumn("Status").AsString().NotNullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}