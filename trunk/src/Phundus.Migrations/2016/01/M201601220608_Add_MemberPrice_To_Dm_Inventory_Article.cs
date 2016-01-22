namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601220608)]
    public class M201601220608_Add_MemberPrice_To_Dm_Inventory_Article : MigrationBase
    {
        public override void Up()
        {
            Rename.Column("Price").OnTable("Dm_Inventory_Article").To("PublicPrice");
            Alter.Table("Dm_Inventory_Article")
                .AddColumn("MemberPrice").AsDecimal().Nullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}