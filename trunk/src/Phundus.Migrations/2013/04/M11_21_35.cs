namespace Phundus.Migrations
{
    using System;

    [Dated(0, 2013, 04, 11, 21, 35)]
    public class M11_21_35 : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Organization").InSchema(SchemaName)
                 .AddColumn("CreateDate").AsDateTime().Nullable();

            Execute.Sql(String.Format("update [Organization] set [CreateDate] = '{0}'",
                DateTime.Now));

            Alter.Table("Organization").InSchema(SchemaName)
                 .AlterColumn("CreateDate").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("CreateDate").FromTable("Organization").InSchema(SchemaName);
        }
    }
}