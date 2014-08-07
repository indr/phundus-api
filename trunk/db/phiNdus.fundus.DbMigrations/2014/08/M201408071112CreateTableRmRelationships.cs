namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201408071112)]
    public class M201408071112CreateTableRmRelationships : MigrationBase
    {
        public override void Up()
        {
            Create.Table("Rm_Relationships")
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("OrganizationId").AsInt32().NotNullable()
                .WithColumn("Status").AsInt32().NotNullable()
                .WithColumn("Timestamp").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}