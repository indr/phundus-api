namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;
    using FluentMigrator.Runner.Extensions;

    [Migration(201601152343)]
    public class M201601152343_Change_UserId_to_UserGuid_on_Rm_Relationships : MigrationBase
    {
        public override void Up()
        {
            Create.Column("UserGuid").OnTable("Rm_Relationships").AsGuid().Nullable();

            Execute.Sql(@"UPDATE [Rm_Relationships] SET [Rm_Relationships].[UserGuid] = [Dm_IdentityAccess_User].[Guid]
FROM [Dm_IdentityAccess_User]
WHERE [Dm_IdentityAccess_User].[Id] = [Rm_Relationships].[UserId]");

            Alter.Column("UserGuid").OnTable("Rm_Relationships").AsGuid().NotNullable();

            Delete.PrimaryKey("PK_Rm_Relationships_OrganizationGuid_UserId").FromTable("Rm_Relationships");
            Delete.Column("UserId").FromTable("Rm_Relationships");
            Create.PrimaryKey("PK_Rm_IdentityAccess_Relationships_OrganizationGuid_UserGuid").OnTable("Rm_Relationships").Columns(new string[] {"OrganizationGuid", "UserGuid"}).NonClustered();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}