namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201206090900)]
    public class M201206090900_DeleteCarts : MigrationBase
    {
        public override void Up()
        {
            Delete.FromTable("Order").InSchema(SchemaName).Row(new {Status = 0});
        }

        public override void Down()
        {
        }
    }
}