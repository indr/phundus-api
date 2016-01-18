namespace Phundus.Migrations
{
    using FluentMigrator;
    using FluentMigrator.Runner.Extensions;

    [Migration(201601181139)]
    public class M201601181139_Clean_applications_and_create_unique_key_constraint : MigrationBase
    {
        public override void Up()
        {
            // Delete applications which are neither approved nor rejected
            Execute.Sql(
                @"delete from Dm_IdentityAccess_Application where RejectDate is null and ApprovalDate is null and  RequestDate < '2016-01-01'");

            // Create unique constraint to prevent more than one pending application per organization per user
            Create.UniqueConstraint()
                .OnTable("Dm_IdentityAccess_Application")
                .Columns(new[] {"OrganizationGuid", "UserGuid", "RejectDate", "ApprovalDate"})
                .NonClustered();

            // Generate approved applications for members which have never filed an application
            Execute.Sql(@"
insert into Dm_IdentityAccess_Application
 ([id], [Version], RequestDate, ApprovalDate, OrganizationGuid, UserGuid)
select 
NEWID(), 1 as [Version], m.ApprovalDate, m.ApprovalDate, m.OrganizationGuid, m.UserGuid
from Dm_IdentityAccess_Membership m
left join Dm_IdentityAccess_Application a
on m.UserGuid = a.UserGuid and m.OrganizationGuid = a.OrganizationGuid
where a.id is null");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}