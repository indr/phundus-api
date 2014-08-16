namespace phiNdus.fundus.DbMigrations
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using FluentMigrator;
    using Microsoft.SqlServer.Server;

    [Migration(201408151820)]
    public class M201408151820MigrateOrderDatesToUtc : HydrationBase
    {
        public override void Up()
        {
            Create.Column("CreatedUtc").OnTable("Order").AsDateTime().Nullable();
            Create.Column("ModifiedUtc").OnTable("Order").AsDateTime().Nullable();
            Create.Column("FromUtc").OnTable("OrderItem").AsDateTime().Nullable();
            Create.Column("ToUtc").OnTable("OrderItem").AsDateTime().Nullable();

            base.Up();

            Delete.Column("CreateDate").FromTable("Order");
            Delete.Column("ModifyDate").FromTable("Order");
            Delete.Column("From").FromTable("OrderItem");
            Delete.Column("To").FromTable("OrderItem");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }

        protected override void Hydrate()
        {
            const string fmtUpdateOrder = "update [Order] set [CreatedUtc] = '{1}', [ModifiedUtc] = {2} where [Id] = {0}";
            
            using (var command = CreateCommand("select [Id], [CreateDate], [ModifyDate] from [Order]"))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var createdUtc = ConvertLocalToUtc(reader["CreateDate"]);

                    DateTime? modifiedUtc = null;
                    if (!reader.IsDBNull(2))
                    {
                        modifiedUtc = ConvertLocalToUtc(reader["ModifyDate"]);
                    }

                    Commands.Add(String.Format(fmtUpdateOrder, reader[0], createdUtc.ToString("yyyy-MM-dd HH:mm:ss"),
                        modifiedUtc.HasValue ? "'" + modifiedUtc.Value.ToString("yyyy-MM-ddTHH:mm:ss") + "'" : "null"));
                }
            }

            const string fmtUpdateOrderItem = "update [OrderItem] set [FromUtc] = '{1}', [ToUtc] = '{2}' where [Id] = '{0}'";
            using (var command = CreateCommand("select [Id], [From], [To] from [OrderItem]"))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var fromUtc = ConvertLocalToUtc(reader["From"]);
                    var local = DateTime.SpecifyKind(Convert.ToDateTime(reader["To"]), DateTimeKind.Local);
                    local = local.AddDays(1).AddSeconds(-1);
                    var toUtc = ConvertLocalToUtc(local);

                    Commands.Add(String.Format(fmtUpdateOrderItem, reader[0],
                            fromUtc.ToString("yyyy-MM-ddTHH:mm:ss"),
                            toUtc.ToString("yyyy-MM-ddTHH:mm:ss")));
                }
            }

            ExecuteCommands();
        }
    }
}