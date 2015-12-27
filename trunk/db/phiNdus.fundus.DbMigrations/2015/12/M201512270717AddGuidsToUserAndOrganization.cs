namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201512270717)]
    public class M201512270717AddGuidsToUserAndOrganization : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("User").InSchema(SchemaName)
                .AddColumn("Guid").AsGuid().NotNullable().WithDefault(SystemMethods.NewGuid).NotNullable();

            Alter.Table("Organization").InSchema(SchemaName)
                .AddColumn("Guid").AsGuid().NotNullable().WithDefault(SystemMethods.NewGuid).NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}