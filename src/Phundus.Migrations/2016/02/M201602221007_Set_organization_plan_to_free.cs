namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602221007)]
    public class M201602221007_Set_organization_plan_to_free : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"UPDATE [Dm_IdentityAccess_Organization] SET [Plan] = 0");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}