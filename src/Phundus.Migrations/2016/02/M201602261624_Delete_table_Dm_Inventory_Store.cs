namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602261624)]
    public class M201602261624_Delete_table_Dm_Inventory_Store : MigrationBase
    {
        public override void Up()
        {
            Delete.Table("Dm_Inventory_Store");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}