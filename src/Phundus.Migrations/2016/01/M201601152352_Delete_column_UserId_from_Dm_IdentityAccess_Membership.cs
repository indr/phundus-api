namespace Phundus.Migrations
{
    using System;
    using System.Data;
    using FluentMigrator;
    using FluentMigrator.Runner.Extensions;

    [Migration(201601160001)]
    public class M201601160001_Delete_column_MemberID_from_Dm_IdentityAccess_Applicatoin : MigrationBase
    {
        public override void Up()
        {
            Delete.Column("MemberId").FromTable("Dm_IdentityAccess_Application").InSchema(SchemaName);
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }

    [Migration(201601152352)]
    public class M201601152352_Delete_column_UserId_from_Dm_IdentityAccess_Membership : MigrationBase
    {
        public override void Up()
        {
            Delete.UniqueConstraint("UC_OrganizationMembership_OrganizationGuid_UserId")
                .FromTable("Dm_IdentityAccess_Membership");
            Delete.ForeignKey("FkOrganizationMembershipToUser").OnTable("Dm_IdentityAccess_Membership");
            Delete.Column("UserId").FromTable("Dm_IdentityAccess_Membership");

            Create.UniqueConstraint("UC_IdentityAccess_Membership_OrganizationGuid_UserGuid")
                .OnTable("Dm_IdentityAccess_Membership")
                .Columns(new[] {"OrganizationGuid", "UserGuid"}).NonClustered();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}