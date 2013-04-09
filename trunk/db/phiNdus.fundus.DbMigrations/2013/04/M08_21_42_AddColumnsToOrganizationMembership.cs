namespace phiNdus.fundus.DbMigrations
{
    using System;

    [Dated(0, 2013, 4, 8, 21, 42)]
    public class M08_21_42_AddColumnsToOrganizationMembership : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("OrganizationMembership").InSchema(SchemaName)
                 .AddColumn("IsLocked").AsBoolean().WithDefaultValue(true);

            Execute.Sql(String.Format("update [{0}].[OrganizationMembership] set [IsLocked] = 0", SchemaName));
        }

        public override void Down()
        {
            Delete.Column("IsLocked").FromTable("OrganizationMembership").InSchema(SchemaName);
        }
    }
}