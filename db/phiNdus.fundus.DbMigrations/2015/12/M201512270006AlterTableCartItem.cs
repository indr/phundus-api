namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201512270006)]
    public class M201512270006AlterTableCartItem : MigrationBase
    {
        public override void Up()
        {
            // ¯\_(ツ)_/¯
            Delete.FromTable("CartItem").InSchema(SchemaName).AllRows();
            Delete.ForeignKey("FK_CartItem_ArticleId_Article_Id").OnTable("CartItem").InSchema(SchemaName);
            Delete.Column("ArticleId").FromTable("CartItem").InSchema(SchemaName);

            Alter.Table("CartItem").InSchema(SchemaName)
                .AddColumn("Article_ArticleId").AsInt32().NotNullable()
                .AddColumn("Article_OrganizationId").AsInt32().NotNullable()
                .AddColumn("Article_Name").AsString().NotNullable()
                .AddColumn("Article_UnitPricePerWeek").AsDecimal().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}