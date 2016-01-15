namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601152218)]
    public class M201601152218_Delete_column_ModifiedBy_from_Order : MigrationBase
    {
        public override void Up()
        {
            Delete.Column("ModifierId").FromTable("Dm_Shop_Order");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}