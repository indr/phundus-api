namespace phiNdus.fundus.DbMigrations
{
    using FluentMigrator;

    [Migration(201407221828)]
    public class M201407221828ChangePrimaryKeyOnOrganizationMembership : MigrationBase
    {
        public override void Up()
        {
            Delete.PrimaryKey("PK_OrganizationMembership").FromTable("OrganizationMembership").InSchema(SchemaName);
            Delete.Column("Id").FromTable("OrganizationMembership").InSchema(SchemaName);
            //Rename.Column("Id").OnTable("OrganizationMembership").InSchema(SchemaName).To("IntId");


            Alter.Table("OrganizationMembership").InSchema(SchemaName)
                .AddColumn("Id").AsGuid().WithDefault(SystemMethods.NewGuid).NotNullable().PrimaryKey();
            //Create.PrimaryKey("PK_OrganizationMembership").OnTable("OrganizationMembership").Column("Id");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}