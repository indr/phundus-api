namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201512271453)]
    public class M201512271453AlterTableRmRelationships : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Rm_Relationships")
                .AddColumn("OrganizationGuid").AsGuid().Nullable();
            Execute.Sql(@"UPDATE [Rm_Relationships] SET [Rm_Relationships].[OrganizationGuid] = [Organization].[Guid]
FROM [Rm_Relationships], [Organization]
WHERE [Rm_Relationships].[OrganizationId] = [Organization].[Id]");
            Alter.Table("Rm_Relationships")
                .AlterColumn("OrganizationGuid").AsGuid().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}