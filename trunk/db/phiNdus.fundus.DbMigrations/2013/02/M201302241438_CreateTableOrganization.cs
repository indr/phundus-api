namespace phiNdus.fundus.DbMigrations
{
    using FluentMigrator;

    [Migration(201302241438)]
    public class M201302241438_CreateTableOrganization : MigrationBase
    {
        public override void Up()
        {
            Create.Table("Organization").InSchema(SchemaName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("Name").AsString(255).NotNullable();

            //Execute.Sql(@"dbcc checkident('Organization', reseed, 1000)");
        }

        public override void Down()
        {
            Delete.Table("Organization").InSchema(SchemaName);
        }
    }
}