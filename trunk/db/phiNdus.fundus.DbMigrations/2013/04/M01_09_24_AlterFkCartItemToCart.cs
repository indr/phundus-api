using System.Data;

namespace phiNdus.fundus.DbMigrations
{
    [Dated(0, 2013, 4, 1, 9, 24)]
    public class M01_09_24_AlterFkCartItemToCart : MigrationBase
    {
        public override void Up()
        {
            Delete.ForeignKey("FkCartItemToCart").OnTable("CartItem").InSchema(SchemaName);

            Create.ForeignKey("FK_CartItemToCart")
                .FromTable("CartItem").InSchema(SchemaName)
                .ForeignColumn("CartId")
                .ToTable("Cart").InSchema(SchemaName)
                .PrimaryColumn("Id")
                .OnDeleteOrUpdate(Rule.Cascade);
        }

        public override void Down()
        {
            // Nothing to do here
        }
    }
}