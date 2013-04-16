using System.Data;
using FluentMigrator;

namespace phiNdus.fundus.DbMigrations
{
    [Migration(201204211700)]
    public class M0008_CreateTableArticle : MigrationBase
    {
        private const string TableName = "Article";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("Type").AsString(127).NotNullable()
                .WithColumn("CreateDate").AsDateTime().NotNullable().WithDefaultValue("getdate()")
                .WithColumn("ParentId").AsInt32().Nullable();

            Create.ForeignKey("FkFieldValueToArticle")
                .FromTable("FieldValue").InSchema(SchemaName)
                .ForeignColumn("ArticleId")
                .ToTable(TableName).InSchema(SchemaName)
                .PrimaryColumn("Id")
                .OnDeleteOrUpdate(Rule.Cascade);

            //Execute.Sql(@"dbcc checkident('Article', reseed, 10000)");
        }

        public override void Down()
        {
            Delete.ForeignKey("FkFieldValueToArticle").OnTable("FieldValue").InSchema(SchemaName);

            Delete.Table(TableName).InSchema(SchemaName);
        }
    }
}