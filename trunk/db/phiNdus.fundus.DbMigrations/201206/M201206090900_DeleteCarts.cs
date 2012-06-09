using FluentMigrator;

namespace phiNdus.fundus.DbMigrations._201206
{
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