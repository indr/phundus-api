namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602070344)]
    public class M201602070344_Recreate_View_Shop_Lessors : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"DROP VIEW [dbo].[View_Shop_Lessors]");
            Execute.Sql(@"CREATE VIEW [dbo].[View_Shop_Lessors]
AS

SELECT
	dbo.Dm_IdentityAccess_Organization.Guid AS LessorGuid,
	dbo.Dm_IdentityAccess_Organization.[Plan] AS LessorType,
	dbo.Dm_IdentityAccess_Organization.Name AS Name,
	dbo.Dm_IdentityAccess_Organization.Address AS Address,
	dbo.Dm_IdentityAccess_Organization.PhoneNumber AS PhoneNumber,
	dbo.Dm_IdentityAccess_Organization.EmailAddress AS EmailAddress,
	dbo.Dm_IdentityAccess_Organization.Settings_PublicRental As PublicRental
FROM
	dbo.Dm_IdentityAccess_Organization

UNION

SELECT
	dbo.Dm_IdentityAccess_User.Guid as LessorGuid,
	-1 AS LessorType,
	dbo.Dm_IdentityAccess_User.FirstName + ' ' +  dbo.Dm_IdentityAccess_User.LastName AS Name,
	dbo.Dm_IdentityAccess_User.Street + CHAR(13) + CHAR(10) + dbo.Dm_IdentityAccess_User.Postcode + CHAR(13) + CHAR(10) + dbo.Dm_IdentityAccess_User.City AS Address,
	dbo.Dm_IdentityAccess_User.MobileNumber AS PhoneNumber,
	dbo.Dm_IdentityAccess_Account.Email AS EmailAddress,
	1 as PublicRental
FROM
	dbo.Dm_IdentityAccess_User INNER JOIN
	dbo.Dm_IdentityAccess_Account ON dbo.Dm_IdentityAccess_User.Id = dbo.Dm_IdentityAccess_Account.Id
");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}