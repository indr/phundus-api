namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;
    using FluentMigrator.Runner.Extensions;

    [Migration(201602020640)]
    public class M201602020640_Create_table_Es_Inventory_Articles : MigrationBase
    {
        public override void Up()
        {
            Create.Table("Es_Inventory_Articles")
                .WithColumn("RowGuid").AsGuid().PrimaryKey()

                .WithColumn("ArticleId").AsInt32().NotNullable()
                .WithColumn("ArticleGuid").AsGuid().NotNullable()
                .WithColumn("CreatedAtUtc").AsDateTime().NotNullable()

                .WithColumn("OwnerGuid").AsGuid().NotNullable()
                .WithColumn("OwnerName").AsString().Nullable()
                .WithColumn("OwnerType").AsInt32().NotNullable()
                .WithColumn("StoreId").AsGuid().NotNullable()

                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Brand").AsString().Nullable()
                .WithColumn("Color").AsString().Nullable()
                .WithColumn("Description").AsString().Nullable()
                .WithColumn("Specification").AsString().Nullable()

                .WithColumn("PublicPrice").AsDecimal().Nullable()
                .WithColumn("MemberPrice").AsDecimal().Nullable()

                .WithColumn("GrossStock").AsInt32().NotNullable();

            Create.UniqueConstraint().OnTable("Es_Inventory_Articles")
                .Columns(new []{"ArticleId"}).NonClustered();
            Create.UniqueConstraint().OnTable("Es_Inventory_Articles")
                .Columns(new []{"ArticleGuid"}).NonClustered();
            Create.UniqueConstraint().OnTable("Es_Inventory_Articles")
                .Columns(new []{"ArticleId", "ArticleGuid"}).NonClustered();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}