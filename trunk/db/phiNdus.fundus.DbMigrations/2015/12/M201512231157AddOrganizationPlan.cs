namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201512231157)]
    public class M201512231157AddOrganizationPlan : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Organization").InSchema(SchemaName)
                .AddColumn("Plan").AsInt32().NotNullable().WithDefaultValue(0);

            Update.Table("Organization").InSchema(SchemaName).Set(new {Plan = 3}).AllRows();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}