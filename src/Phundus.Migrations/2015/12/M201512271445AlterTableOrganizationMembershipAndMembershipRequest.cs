namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201512271445)]
    public class M201512271445AlterTableOrganizationMembershipAndMembershipRequest : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("OrganizationMembership")
                .AddColumn("OrganizationGuid").AsGuid().Nullable();
            Execute.Sql(@"UPDATE [OrganizationMembership] SET [OrganizationMembership].[OrganizationGuid] = [Organization].[Guid]
FROM [OrganizationMembership], [Organization]
WHERE [OrganizationMembership].[OrganizationId] = [Organization].[Id]");
            Alter.Table("OrganizationMembership")
                .AlterColumn("OrganizationGuid").AsGuid().NotNullable();

            Alter.Table("MembershipRequest")
                .AddColumn("OrganizationGuid").AsGuid().Nullable();
            Execute.Sql(@"UPDATE [MembershipRequest] SET [MembershipRequest].[OrganizationGuid] = [Organization].[Guid]
FROM [MembershipRequest], [Organization]
WHERE [MembershipRequest].[OrganizationId] = [Organization].[Id]");
            Alter.Table("MembershipRequest")
                .AlterColumn("OrganizationGuid").AsGuid().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}