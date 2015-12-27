namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201512271236)]
    public class M201512271236AlterTableOrderToLessor : MigrationBase
    {
        const string TableName = "Order";

        public override void Up()
        {
            Alter.Table(TableName).InSchema(SchemaName)            
                .AddColumn("Lessor_LessorId").AsGuid().Nullable()
                .AddColumn("Lessor_Name").AsString().Nullable();

            Execute.Sql(
                @"UPDATE [Order] SET [Order].[Lessor_LessorId] = [Organization].[Guid], [Order].[Lessor_Name] = [Order].[Organization_Name]
FROM [Order], [Organization]
WHERE [Order].[OrganizationId] = [Organization].[Id]");

            Alter.Table(TableName).InSchema(SchemaName)
                .AlterColumn("Lessor_LessorId").AsGuid().NotNullable()
                .AlterColumn("Lessor_Name").AsString().NotNullable();

            Delete.ForeignKey("Fk_OrderToOrganization").OnTable(TableName).InSchema(SchemaName);
            Delete.Column("OrganizationId").FromTable(TableName).InSchema(SchemaName);
            Delete.Column("Organization_Name").FromTable(TableName).InSchema(SchemaName);
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}