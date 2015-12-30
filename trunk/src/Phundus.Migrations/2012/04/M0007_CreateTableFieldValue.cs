namespace Phundus.Migrations
{
    using System;
    using System.Data;
    using FluentMigrator;

    [Migration(201204211658)]
    public class M0007_CreateTableFieldValue : MigrationBase
    {
        private const string TableName = "FieldValue";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey() // HiLo
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("FieldDefinitionId").AsInt32().NotNullable()
                .WithColumn("IsDiscriminator").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("BooleanValue").AsBoolean().Nullable()
                .WithColumn("TextValue").AsString(Int32.MaxValue).Nullable()
                .WithColumn("IntegerValue").AsInt32().Nullable()
                .WithColumn("DecimalValue").AsDecimal(18, 3).Nullable()
                .WithColumn("DateTimeValue").AsDateTime().Nullable()
                .WithColumn("ArticleId").AsInt32().Nullable();

            Create.ForeignKey("FkFieldValueToFieldDefinition")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("FieldDefinitionId")
                .ToTable("FieldDefinition").InSchema(SchemaName)
                .PrimaryColumn("Id")
                .OnDeleteOrUpdate(Rule.None);
        }

        public override void Down()
        {
            Delete.ForeignKey("FkFieldValueToFieldDefinition").OnTable(TableName).InSchema(SchemaName);

            Delete.Table(TableName).InSchema(SchemaName);
        }
    }
}