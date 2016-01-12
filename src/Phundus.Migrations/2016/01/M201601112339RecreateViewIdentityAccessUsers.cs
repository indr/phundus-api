namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601112339)]
    public class M201601112339RecreateViewIdentityAccessUsers : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"DROP VIEW [dbo].[View_IdentityAccess_Users]");
            Execute.Sql(@"
CREATE VIEW [dbo].[View_IdentityAccess_Users]
AS
SELECT     dbo.Dm_IdentityAccess_User.Id AS UserId, dbo.Dm_IdentityAccess_User.Guid AS UserGuid, dbo.Dm_IdentityAccess_User.RoleId, dbo.Dm_IdentityAccess_Account.Email AS EmailAddress, 
                      dbo.Dm_IdentityAccess_User.FirstName, dbo.Dm_IdentityAccess_User.LastName, dbo.Dm_IdentityAccess_User.Street, dbo.Dm_IdentityAccess_User.Postcode, dbo.Dm_IdentityAccess_User.City, 
                      dbo.Dm_IdentityAccess_User.MobileNumber AS PhoneNumber, dbo.Dm_IdentityAccess_User.JsNumber AS JsNummer, dbo.Dm_IdentityAccess_Account.IsApproved, dbo.Dm_IdentityAccess_Account.IsLockedOut, 
                      dbo.Dm_IdentityAccess_Account.CreateDate AS SignedUpAtUtc, dbo.Dm_IdentityAccess_Account.LastLogOnDate AS LastLogInAtUtc, 
                      dbo.Dm_IdentityAccess_Account.LastPasswordChangeDate AS LastPasswordChangeAtUtc, dbo.Dm_IdentityAccess_Account.LastLockoutDate AS LastLockOutAtUtc
FROM         dbo.Dm_IdentityAccess_User INNER JOIN
                      dbo.Dm_IdentityAccess_Account ON dbo.Dm_IdentityAccess_User.Id = dbo.Dm_IdentityAccess_Account.Id
");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}