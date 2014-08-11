namespace phiNdus.fundus.DbMigrations
{
    using FluentMigrator;

    [Migration(201408111404)]
    public class M201408111404ChangePrimaryKeyOnOrderItem : MigrationBase
    {
        public override void Up()
        {
            Delete.PrimaryKey("PK_OrderItem").FromTable("OrderItem");
            Delete.Column("Id").FromTable("OrderItem");
            Alter.Table("OrderItem").AddColumn("Id").AsGuid()
                .WithDefault(SystemMethods.NewGuid).NotNullable().PrimaryKey();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}