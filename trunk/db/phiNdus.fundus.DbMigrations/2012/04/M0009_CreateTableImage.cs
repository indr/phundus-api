using System.Data;
using FluentMigrator;

namespace phiNdus.fundus.DbMigrations
{
    [Migration(201204211703)]
    public class M0009_CreateTableImage : MigrationBase
    {
        private const string TableName = "Image";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey() // HiLo
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("ArticleId").AsInt32().NotNullable()
                .WithColumn("IsPreview").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("Length").AsInt64().NotNullable()
                .WithColumn("Type").AsString(31)
                .WithColumn("FileName").AsString(255);

            Create.ForeignKey("FkImageToArticle")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("ArticleId")
                .ToTable("Article").InSchema(SchemaName)
                .PrimaryColumn("Id")
                .OnDeleteOrUpdate(Rule.None);
        }

        public override void Down()
        {
            Delete.ForeignKey("FkImageToArticle").OnTable(TableName).InSchema(SchemaName);

            Delete.Table(TableName).InSchema(SchemaName);
        }
    }
}