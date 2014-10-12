namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201410111611)]
    public class M201410111611CreateTableStoredEvents : MigrationBase
    {
        public override void Up()
        {
            Create.Table("StoredEvents")
                .WithColumn("EventId").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("EventGuid").AsGuid().NotNullable()
                .WithColumn("TypeName").AsString().NotNullable()
                .WithColumn("OccuredOnUtc").AsDateTime().Nullable()
                .WithColumn("AggregateId").AsGuid().Nullable()
                .WithColumn("Serialization").AsBinary(Int32.MaxValue).Nullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}