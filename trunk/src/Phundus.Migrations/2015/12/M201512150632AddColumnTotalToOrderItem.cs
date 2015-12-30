namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201512150632)]
    public class M201512150632AddColumnTotalToOrderItem : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("OrderItem").AddColumn("Total").AsDecimal().Nullable();

            Execute.Sql(@"UPDATE [OrderItem] SET [Total] = ROUND((DATEDIFF(DAY, [FromUtc], [ToUtc])) * ([UnitPrice] * [Amount] / 7), 2)");

            Alter.Table("OrderItem").AlterColumn("Total").AsDecimal().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}