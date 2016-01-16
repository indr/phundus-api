namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601160747)]
    public class M201601160747_Create_View_IdentityAccess_Applications : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"
CREATE VIEW [dbo].[View_IdentityAccess_Applications]
AS

SELECT dbo.Dm_IdentityAccess_Application.Id AS ApplicationGuid
	, dbo.Dm_IdentityAccess_Application.OrganizationGuid
	, dbo.Dm_IdentityAccess_Application.UserGuid
	, dbo.Dm_IdentityAccess_Application.RequestDate AS RequestedAtUtc
	, dbo.Dm_IdentityAccess_Application.ApprovalDate AS ApprovedAtUtc
	, dbo.Dm_IdentityAccess_Application.RejectDate AS RejectedAtUtc
    , dbo.Dm_IdentityAccess_Application.CustomMemberNumber AS CustomMemberNumber
	, dbo.Dm_IdentityAccess_User.FirstName
	, dbo.Dm_IdentityAccess_User.LastName
	, dbo.Dm_IdentityAccess_Account.Email AS EmailAddress    
FROM dbo.Dm_IdentityAccess_Application
	INNER JOIN dbo.Dm_IdentityAccess_User ON dbo.Dm_IdentityAccess_Application.UserGuid = dbo.Dm_IdentityAccess_User.Guid
	INNER JOIN dbo.Dm_IdentityAccess_Account ON dbo.Dm_IdentityAccess_User.Id = dbo.Dm_IdentityAccess_Account.Id
");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}