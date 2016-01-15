namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601151250)]
    public class M201601151250AddUserGuidToApplicationAndMembership : MigrationBase
    {
        public override void Up()
        {
            Create.Column("UserGuid").OnTable("Dm_IdentityAccess_Application").AsGuid().Nullable();
            Create.Column("UserGuid").OnTable("Dm_IdentityAccess_Membership").AsGuid().Nullable();

            Execute.Sql(@"UPDATE [Dm_IdentityAccess_Application] SET [Dm_IdentityAccess_Application].[UserGuid] = [Dm_IdentityAccess_User].[Guid]
FROM [Dm_IdentityAccess_User]
WHERE [Dm_IdentityAccess_User].[Id] = [Dm_IdentityAccess_Application].[MemberId]");
            Execute.Sql(@"UPDATE [Dm_IdentityAccess_Membership] SET [Dm_IdentityAccess_Membership].[UserGuid] = [Dm_IdentityAccess_User].[Guid]
FROM [Dm_IdentityAccess_User]
WHERE [Dm_IdentityAccess_User].[Id] = [Dm_IdentityAccess_Membership].[UserId]");

            //Alter.Column("UserGuid").OnTable("Dm_IdentityAccess_Application").AsGuid().NotNullable();
            //Alter.Column("UserGuid").OnTable("Dm_IdentityAccess_Membership").AsGuid().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}