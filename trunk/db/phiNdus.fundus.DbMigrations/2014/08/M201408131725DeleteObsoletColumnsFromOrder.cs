namespace phiNdus.fundus.DbMigrations
{
    using FluentMigrator;

    [Migration(201408131725)]
    public class M201408131725DeleteObsoletColumnsFromOrder : MigrationBase
    {
        public override void Up()
        {
            Delete.ForeignKey("FkOrderToReserver").OnTable("Order");
            Delete.ForeignKey("FkOrderToModifier").OnTable("Order");

            Delete.Column("ReserverId").FromTable("Order");
            Delete.Column("ModifierId").FromTable("Order");
            Delete.Column("ModifyDate").FromTable("Order");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}