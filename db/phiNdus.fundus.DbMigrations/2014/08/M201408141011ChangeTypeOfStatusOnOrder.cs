namespace phiNdus.fundus.DbMigrations
{
    using FluentMigrator;

    [Migration(201408141011)]
    public class M201408141011AlterStatusOnOrderAsInt32 : MigrationBase
    {
        public override void Up()
        {
            Alter.Column("Status").OnTable("Order").AsInt32().NotNullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}