namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201512270846)]
    public class M201512270846AddOwnerToArticle : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Article").InSchema(SchemaName)
                .AddColumn("Owner_OwnerId").AsGuid().Nullable()
                .AddColumn("Owner_Name").AsString().Nullable();

            Execute.Sql(
                @"UPDATE [Article] SET [Article].[Owner_OwnerId] = [Organization].[Guid], [Article].[Owner_Name] = [Organization].[Name]
FROM [Article], [Organization]
WHERE [Article].[OrganizationId] = [Organization].[Id]");

            Alter.Table("Article").InSchema(SchemaName)
                .AlterColumn("Owner_OwnerId").AsGuid().NotNullable()
                .AlterColumn("Owner_Name").AsString().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}