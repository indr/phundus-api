namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201604150407)]
    public class M201604150407_Add_column_Tags_to_Dm_Inventory_Article : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Dm_Inventory_Article")
                .AddColumn("Tags").AsMaxString().Nullable();
        }
    }
}