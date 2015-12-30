namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201302252319)]
    public class M201302252319_MoveUsersToDefaultOrganization : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(
                @"insert into [OrganizationMembership] (Id, [Version], UserId, OrganizationId, [Role])
select Id, 1, Id, 1001, RoleId - 1 from [User]
where Id not in (select UserId from [OrganizationMembership])");
        }

        public override void Down()
        {
            
        }
    }
}