namespace phiNdus.fundus.DbMigrations
{
    using System.Data;
    using FluentMigrator;

    [Migration(201302252131)]
    public class M201302252131_CreateTableOrganizationMembership : MigrationBase
    {
        private const string TableName = "OrganizationMembership";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey()  // HiLo
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("OrganizationId").AsInt32().NotNullable()
                .WithColumn("Role").AsInt32().NotNullable().WithDefaultValue(0);

            Create.ForeignKey("FkOrganizationMembershipToUser")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("UserId")
                .ToTable("User").InSchema(SchemaName)
                .PrimaryColumn("Id")
                .OnDeleteOrUpdate(Rule.Cascade);

            Create.ForeignKey("FkOrganizationMembershipToOrganization")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("OrganizationId")
                .ToTable("Organization").InSchema(SchemaName)
                .PrimaryColumn("Id")
                .OnDeleteOrUpdate(Rule.Cascade);
        }

        public override void Down()
        {
            Delete.ForeignKey("FkOrganizationMembershipToOrganization");
            Delete.ForeignKey("FkOrganizationMembershipToUser");
            Delete.Table(TableName).InSchema(SchemaName);
        }
    }
}