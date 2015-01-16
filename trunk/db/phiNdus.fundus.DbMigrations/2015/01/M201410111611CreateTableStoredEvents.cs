namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201501021140)]
    public class M201501021140CreateTableStoredEvents : MigrationBase
    {
        public override void Up()
        {
            Create.Table("SagaStoredEvents")
                .WithColumn("EventId").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("EventGuid").AsGuid().NotNullable()
                .WithColumn("TypeName").AsString(255).NotNullable()
                .WithColumn("OccuredOnUtc").AsDateTime().Nullable()
                .WithColumn("StreamName").AsString(255).Nullable()
                .WithColumn("StreamVersion").AsInt64().Nullable()
                .WithColumn("Serialization").AsBinary(Int32.MaxValue).Nullable();

            Create.UniqueConstraint().OnTable("SagaStoredEvents").Columns(new[] {"StreamName", "StreamVersion"});
        }

        public override void Down()
        {
        }
    }

    [Migration(201501160956)]
    public class M201501160956AddColumnStockIdToArticle : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Article").AddColumn("StockId").AsString().Nullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}