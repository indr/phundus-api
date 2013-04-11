namespace phiNdus.fundus.DbMigrations
{
    using System;

    [Dated(0, 2013, 04, 11, 21, 38)]
    public class M11_21_38 : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("OrganizationMembership").InSchema(SchemaName)
                 .AddColumn("RequestDate").AsDateTime().Nullable()
                 .AddColumn("IsApproved").AsBoolean().NotNullable().WithDefaultValue(false)
                 .AddColumn("ApprovalDate").AsDateTime().Nullable()
                 .AddColumn("LastLockoutDate").AsDateTime().Nullable();

            Execute.Sql(String.Format(@"update [{0}].[OrganizationMembership] set " +
                                      "[RequestDate] = '{1}', " +
                                      "[IsApproved] = 1, " +
                                      "[ApprovalDate] = '{1}'",
                                      SchemaName, DateTime.Now));

            Alter.Table("OrganizationMembership").InSchema(SchemaName)
                 .AlterColumn("RequestDate").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("RequestDate").FromTable("OrganizationMembership").InSchema(SchemaName);
            Delete.Column("IsApproved").FromTable("OrganizationMembership").InSchema(SchemaName);
            Delete.Column("ApprovalDate").FromTable("OrganizationMembership").InSchema(SchemaName);
            Delete.Column("LastLockoutDate").FromTable("OrganizationMembership").InSchema(SchemaName);
        }
    }
}