namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201408170657)]
    public class M201408170657MigrateArticleToUnitPriceAndTextOnOrderItem : HydrationBase
    {
        public override void Up()
        {
            Create.Column("UnitPrice").OnTable("OrderItem").AsDecimal().Nullable();
            Create.Column("Text").OnTable("OrderItem").AsString(255).Nullable();

            base.Up();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }

        protected override void Hydrate()
        {
            const string fmtUpdate = "update [orderitem] set [text] = '{1}', [unitprice] = {2} where [articleid] = {0}";

            using (var command = CreateCommand("select [id], [name], [price] from [article]"))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Commands.Add(String.Format(fmtUpdate, reader.GetInt32(0), reader.GetString(1).Replace("'", "''"),
                        reader.GetDecimal(2).ToString("0.00")));
                }
            }

            ExecuteCommands();
        }
    }
}