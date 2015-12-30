namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201204211654)]
    public class M0005_CreateTableSetting : MigrationBase
    {
        private const string TableName = "Setting";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey() // HiLo
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("Key").AsString(255).NotNullable()
                .WithColumn("StringValue").AsString(Int32.MaxValue).Nullable()
                .WithColumn("DecimalValue").AsDecimal(18, 3).Nullable()
                .WithColumn("IntegerValue").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Table(TableName).InSchema(SchemaName);
        }
    }
}